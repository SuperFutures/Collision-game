
  	using UnityEngine; 
	using System; 
	using System.Collections; 
	using System.Collections.Generic;
	using System.Threading;

    public class MessageEvent
    {
		public struct Pair
		{
			public object obj;
			public string funcname;
			public System.Reflection.MethodInfo method;
		};
		
		public struct EventObj
		{
			public Pair info;
			public object[] args;
		};
		
    	static Dictionary<string, List<Pair>> events_out = new Dictionary<string, List<Pair>>();
		
		static LinkedList<EventObj> firedEvents_out = new LinkedList<EventObj>();
		static LinkedList<EventObj> doingEvents_out = new LinkedList<EventObj>();
		
    	static Dictionary<string, List<Pair>> events_in = new Dictionary<string, List<Pair>>();
		
		static LinkedList<EventObj> firedEvents_in = new LinkedList<EventObj>();
		static LinkedList<EventObj> doingEvents_in = new LinkedList<EventObj>();

		static bool _isPauseOut = false;
	
		public MessageEvent()
		{
		}
		
		public static void clear()
		{
			events_out.Clear();
			events_in.Clear();
			clearFiredEvents();
		}

		public static void clearFiredEvents()
		{
			monitor_Enter(events_out);
			firedEvents_out.Clear();
			monitor_Exit(events_out);
			
			doingEvents_out.Clear();
			
			monitor_Enter(events_in);
			firedEvents_in.Clear();
			monitor_Exit(events_in);
			
			doingEvents_in.Clear();
			
			_isPauseOut = false;
		}
		
		public static void pause()
		{
			_isPauseOut = true;
		}

		public static void resume()
		{
			_isPauseOut = false;
		}

		public static bool isPause()
		{
			return _isPauseOut;
		}

		public static void monitor_Enter(object obj)
		{
			Monitor.Enter(obj);
		}

		public static void monitor_Exit(object obj)
		{
			Monitor.Exit(obj);
		}
		
		public static bool hasRegisterOut(string eventname)
		{
			return _hasRegister(events_out, eventname);
		}

		public static bool hasRegisterIn(string eventname)
		{
			return _hasRegister(events_in, eventname);
		}
		
		private static bool _hasRegister(Dictionary<string, List<Pair>> events, string eventname)
		{
			bool has = false;
			
			monitor_Enter(events);
			has = events.ContainsKey(eventname);
			monitor_Exit(events);
			
			return has;
		}
		
	
		public static bool registerOut(string eventname, object obj, string funcname)
		{
			return register(events_out, eventname, obj, funcname);
		}

	
		public static bool registerIn(string eventname, object obj, string funcname)
		{
			return register(events_in, eventname, obj, funcname);
		}
		
		private static bool register(Dictionary<string, List<Pair>> events, string eventname, object obj, string funcname)
		{
			deregister(events, eventname, obj, funcname);
			List<Pair> lst = null;
			
			Pair pair = new Pair();
			pair.obj = obj;
			pair.funcname = funcname;
			pair.method = obj.GetType().GetMethod(funcname);
			if(pair.method == null)
			{
				
				return false;
			}
			
			monitor_Enter(events);
			if(!events.TryGetValue(eventname, out lst))
			{
				lst = new List<Pair>();
				lst.Add(pair);
				events.Add(eventname, lst);
				monitor_Exit(events);
				return true;
			}
			
			lst.Add(pair);
			monitor_Exit(events);
			return true;
		}

		public static bool deregisterOut(string eventname, object obj, string funcname)
		{
			return deregister(events_out, eventname, obj, funcname);
		}

		public static bool deregisterIn(string eventname, object obj, string funcname)
		{
			return deregister(events_in, eventname, obj, funcname);
		}
		
		private static bool deregister(Dictionary<string, List<Pair>> events, string eventname, object obj, string funcname)
		{
			monitor_Enter(events);
			List<Pair> lst = null;
			
			if(!events.TryGetValue(eventname, out lst))
			{
				monitor_Exit(events);
				return false;
			}
			
			for(int i=0; i<lst.Count; i++)
			{
				if(obj == lst[i].obj && lst[i].funcname == funcname)
				{
					lst.RemoveAt(i);
					monitor_Exit(events);
					return true;
				}
			}
			
			monitor_Exit(events);
			return false;
		}

		public static bool deregisterOut(object obj)
		{
			return deregister(events_out, obj);
		}

		public static bool deregisterIn(object obj)
		{
			return deregister(events_in, obj);
		}
		
		private static bool deregister(Dictionary<string, List<Pair>> events, object obj)
		{
			monitor_Enter(events);
			
			var iter = events.GetEnumerator();
			while (iter.MoveNext())
			{
				List<Pair> lst = iter.Current.Value;
				// 从后往前遍历，以避免中途删除的问题
				for (int i = lst.Count - 1; i >= 0; i--)
				{
					if (obj == lst[i].obj)
					{
						lst.RemoveAt(i);
					}
				}
			}
			
			monitor_Exit(events);
			return true;
		}

	
		public static void fireOut(string eventname, params object[] args)
		{
			fire_(events_out, firedEvents_out, eventname, args);
		}

	
		public static void fireIn(string eventname, params object[] args)
		{
			fire_(events_in, firedEvents_in, eventname, args);
		}

		/*
			触发渲染表现层都能够收到的事件
		*/
		public static void fireAll(string eventname, params object[] args)
		{
			fire_(events_in, firedEvents_in, eventname, args);
			fire_(events_out, firedEvents_out, eventname, args);
		}
		
		private static void fire_(Dictionary<string, List<Pair>> events, LinkedList<EventObj> firedEvents, string eventname, object[] args)
		{
			monitor_Enter(events);
			List<Pair> lst = null;
			
			if(!events.TryGetValue(eventname, out lst))
			{				
				monitor_Exit(events);
				return;
			}
			
			for(int i=0; i<lst.Count; i++)
			{
				EventObj eobj = new EventObj();
				eobj.info = lst[i];
				eobj.args = args;
				firedEvents.AddLast(eobj);
			}
			
			monitor_Exit(events);
		}
		
		public static void processOutEvents()
		{
			monitor_Enter(events_out);

			if(firedEvents_out.Count > 0)
			{
				var iter = firedEvents_out.GetEnumerator();
				while (iter.MoveNext())
				{
					doingEvents_out.AddLast(iter.Current);
				}

				firedEvents_out.Clear();
			}

			monitor_Exit(events_out);

			while (doingEvents_out.Count > 0 && !_isPauseOut) 
			{

				EventObj eobj = doingEvents_out.First.Value;

			
				try
				{
					eobj.info.method.Invoke (eobj.info.obj, eobj.args);
				}
	            catch (Exception e)
	            {
	            	
	            }
            
				if(doingEvents_out.Count > 0)
					doingEvents_out.RemoveFirst();
			}
		}
		
		public static void processInEvents()
		{
			monitor_Enter(events_in);

			if(firedEvents_in.Count > 0)
			{
				var iter = firedEvents_in.GetEnumerator();
				while (iter.MoveNext())
				{
					doingEvents_in.AddLast(iter.Current);
				}

				firedEvents_in.Clear();
			}

			monitor_Exit(events_in);

			while (doingEvents_in.Count > 0) 
			{
				
				EventObj eobj = doingEvents_in.First.Value;
				
			
				try
				{
					eobj.info.method.Invoke (eobj.info.obj, eobj.args);
				}
	            catch (Exception e)
	            {
	            	
	            }
	            
				if(doingEvents_in.Count > 0)
					doingEvents_in.RemoveFirst();
			}
		}
	
    }

