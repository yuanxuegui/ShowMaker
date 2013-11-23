using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShowMaker.Desktop.Models.Domain;

namespace ShowMaker.Desktop.Models.Engine
{
    public interface ITimelineEngine
    {
        void Init(ITimelineContext timelineContext);
        void Start();
        void Pause();
        void Stop();
        void Go(int timeTicked);
        event Func<int, bool> TimeTickEvent;
    }
}
