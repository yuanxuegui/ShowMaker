using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Timers;
using ShowMaker.Desktop.Domain;
using ShowMaker.Desktop.Util;
using System.Collections.ObjectModel;

namespace ShowMaker.Desktop.Engine
{
    public class DefaultTimeLineEngine : ITimelineEngine
    {
        #region Fields

        private static ITimelineEngine instance = new DefaultTimeLineEngine();
        private ITimelineContext timelineContext;
        private Timeline timeline;
        private Timer timer = new Timer();
        private static object lockObject = new Object();
        private static int checkUpDateLock = 0;
        private long period;
        private long max;
        private int timeTicked;

        #endregion

        #region Constructors

        private DefaultTimeLineEngine() { }

        #endregion

        #region Indexers
        #endregion

        #region Properties

        public Timeline Timeline
        {
            get { return timeline; }
            set { timeline = value; }
        }

        #endregion

        #region Methods

        public static ITimelineEngine Instance()
        {
            return instance;
        }

        private void queryAndExecute()
        {
            TimePoint timePoint = timeline.GetItemByKey(timeTicked);
            if (timePoint != null)
            {
                ObservableCollection<Command> commands = timePoint.CommandItems;
                if (commands != null && commands.Count > 0)
                {
                    ICommandContext cmdCtx = new CommandContext(timelineContext);
                    foreach (Command cmd in commands)
                    {
                        cmd.Execute(cmdCtx);
                    }
                }
            }
        }

        private void timer_Elapsed(object source, ElapsedEventArgs e)
        {
            lock (lockObject)
            {
                if (checkUpDateLock == 0)
                    checkUpDateLock = 1;
                else
                    return;
            }
            TimeTickEvent(timeTicked);
            queryAndExecute();

            timeTicked++;
            if (timeTicked > max)
                timer.Stop();

            lock (lockObject)
            {
                checkUpDateLock = 0;
            }
        }

        public void Init(ITimelineContext timelineContext)
        {
            this.timelineContext = timelineContext;
            this.timeline = timelineContext.GetTimeline();

            if (timeline == null || timeline.PropertyItem == null)
            {
                return;
            }

            string unit = (string)timeline.GetPropertyValue(Constants.TIME_UNIT_KEY);
            if (Constants.TIME_UNIT_MS.Equals(unit))
            {
                period = 1;
            }
            else if (Constants.TIME_UNIT_S.Equals(unit))
            {
                period = 1000;
            }
            else if (Constants.TIME_UNIT_M.Equals(unit))
            {
                period = 60000;
            }
            max = long.Parse(timeline.GetPropertyValue(Constants.TIME_MAX_KEY));

            // Init Timer
            timer.Interval = period;
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            timer.AutoReset = true;
        }

        public void Start()
        {
            Debug.WriteLine("Timeline Engine Start");
            timer.Start();
        }

        public void Pause()
        {
            Debug.WriteLine("Timeline Engine Pause");
            timer.Stop();
        }

        public void Stop()
        {
            Debug.WriteLine("Timeline Engine Stop");
            timer.Stop();
            timeTicked = 0;
        }

        public void Go(int timeTicked)
        {
            Debug.WriteLine("Timeline Engine Go " + timeTicked);
            this.timeTicked = timeTicked;
        }

        #endregion

        #region Events

        public event Func<int, bool> TimeTickEvent;

        #endregion
    }
}
