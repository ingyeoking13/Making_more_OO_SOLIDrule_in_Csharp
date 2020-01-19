using System;
using System.Collections.Generic;

namespace _3_객체간_통신을_하는_법_EventAggregator.PoorEventCenter
{
    public class NotifyEventArgs : EventArgs
    {
        object Data { get; set; }
    }

    public sealed class EventCenter
    {
        static EventCenter _instance = null;
        static readonly object _padlock = new object();

        public delegate void standardEventHandler(object sender, NotifyEventArgs e);

        // 이벤트name당 여러 개의 이벤트핸들러를 등록시킬 수 있다.
        public static Dictionary<string, List<standardEventHandler>> _dicEventHandlers 
            =new Dictionary<string, List<standardEventHandler>>();
        EventCenter()
        {
        }

        public static EventCenter defaultCenter
        {
            get
            {
                lock (_padlock)
                {
                    if (null == _instance)
                    {
                        _instance = new EventCenter();
                    }
                    return _instance;
                }
            }
        }

        public void registerHandler(string opname, standardEventHandler eventHandler)
        {
            if (null == eventHandler)
                return;

            List<standardEventHandler> listHandler = null;
            if (_dicEventHandlers.ContainsKey(opname))
                listHandler = _dicEventHandlers[opname];

            if (null == listHandler)
            {
                listHandler = new List<standardEventHandler>();
                _dicEventHandlers.Add(opname, listHandler);
            }
            if (!listHandler.Contains(eventHandler))
                listHandler.Add(eventHandler);
        }


        public void unregisterHandler(string opname, standardEventHandler eventHandler)
        {
            if (null == eventHandler)
                return;

            List<standardEventHandler> listHandler = null;
            if (_dicEventHandlers.ContainsKey(opname))
                listHandler = _dicEventHandlers[opname];

            if (null == listHandler)
                return;

            listHandler = new List<standardEventHandler>();
            listHandler.Remove(eventHandler);
        }

        public void postNotification(string opname, object sender, NotifyEventArgs e)
        {
            if (_dicEventHandlers.ContainsKey(opname))
            {
                List<standardEventHandler> listHandler = _dicEventHandlers[opname];
                // notification실행 도중 unregister호출하는 경우에 대비하기 위해 foreach --> for 수정(예: GraphicHTMLTextBox);
                int cnt = listHandler.Count;
                for (int i = cnt - 1; i >= 0; i--)
                {
                    standardEventHandler handler = listHandler[i];
                    try
                    {
                        handler(sender, e);
                    }
                    catch (Exception ex)
                    {
                        string st = "event save exception : " + ex.ToString();
                        Console.WriteLine(st);
                        Console.WriteLine(ex);
                    }
                }
            }
        }
        public void registerHandler<T>(standardEventHandler eventHandler)
            => registerHandler(typeof(T).Name, eventHandler);

        public void unregisterHandler<T>(standardEventHandler eventHandler)
            => registerHandler(typeof(T).Name, eventHandler);
        public void postNotification<T>(object sender, NotifyEventArgs e)
            => postNotification(typeof(T).Name, sender, e);
    }
}
