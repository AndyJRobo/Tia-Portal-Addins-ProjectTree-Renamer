using Siemens.Engineering.HW;
using System.Collections.Generic;

namespace ProjectTreeRenamer.Utility
{
    public class UDeviceGroup
    {
        private readonly DeviceUserGroup _deviceUserGroup;
        public List<Device> Devices { get; private set; }
        public List<UDeviceGroup> Groups { get; private set; }

        public UDeviceGroup(DeviceUserGroup deviceUserGroup)
        {
            _deviceUserGroup = deviceUserGroup;
            Inititalise();
        }

        private void Inititalise()
        {
            Devices = new List<Device>();
            foreach (Device device in _deviceUserGroup.Devices)
            {
                Devices.Add(device);
            }
            Groups = new List<UDeviceGroup>();
            foreach (DeviceUserGroup deviceUserGroup in _deviceUserGroup.Groups)
            {
                Groups.Add(new UDeviceGroup(deviceUserGroup));
            }
        }

        public void Rename(string find, string replace)
        {
            _deviceUserGroup.Name = _deviceUserGroup.Name.Replace(find, replace);
            foreach (Device device in Devices)
            {
                RenameDevice(device, find, replace);
            }
            foreach (UDeviceGroup group in Groups)
            {
                group.Rename(find, replace);
            }
        }

        private void RenameDevice(Device device, string find, string replace)
        {
            device.Name = device.Name.Replace(find, replace);
            foreach (DeviceItem deviceItem in device.DeviceItems)
            {
                deviceItem.Name = deviceItem.Name.Replace(find, replace);
            }
        }
    }
}