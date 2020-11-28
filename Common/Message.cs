using System.Collections.Generic;
using System;

namespace Common
{
    public interface IMessage {
        enum command{};
        public bool Add<T>();
    }
    public class Minutes{
        int minutes{get;set;}
    }
    public class Window{
        DateTime startTime {get;set;}
        DateTime endTime {get;set;}
    }
}