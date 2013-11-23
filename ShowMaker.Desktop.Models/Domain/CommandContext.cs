using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShowMaker.Desktop.Models.Util;

namespace ShowMaker.Desktop.Models.Domain
{
    public class CommandContext : ICommandContext
    {
        private ITimelineContext timelineContextField;

        public ITimelineContext TimelineContext
        {
            get { return timelineContextField; }
            set { timelineContextField = value; }
        }

        public CommandContext(ITimelineContext timelineContext)
        {
            timelineContextField = timelineContext;
        }

        public string BuildCommand(Command command)
        {
            string deviceId = command.DeviceId;
            string operationName = command.OperationName;
            Device device = timelineContextField.GetArea().GetItemByKey(deviceId);
            Operation operation = device.GetItemByKey(operationName);
            string commandContent = operation.Command;
            // {server.id}
            commandContent = commandContent.Replace("{" + Constants.SERVER_ID_KEY + "}", timelineContextField.GetPropertyValue(Constants.SERVER_ID_KEY));
            // {deviceId}
            commandContent = commandContent.Replace("{" + Constants.CMD_DEVICE_ID +  "}", device.Id);
            int length = device.Id.Length;
            foreach (Parameter parameter in operation.ParameterItems)
            {
                string name = parameter.Name;
                string value = command.GetPropertyValue(name);
                if (name != null && value != null)
                {
                    length += value.Length;
                    commandContent = commandContent.Replace("{" + name + "}", value);
                }
            }
            //{length}
            commandContent = commandContent.Replace("{" + Constants.CMD_LENGTH + "}", "" + length);
            return commandContent;
        }
    }
}
