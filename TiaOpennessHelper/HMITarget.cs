using Siemens.Engineering.Hmi;
using Siemens.Engineering.Hmi.Communication;
using Siemens.Engineering.Hmi.Cycle;
using Siemens.Engineering.Hmi.Screen;
using Siemens.Engineering.Hmi.Tag;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace TiaOpennessHelper
{
    public partial class OpennessHelper
    {
        #region GetSpecific

        /// <summary>
        /// Returns a reference to the screen with given name if found, otherwise null.
        /// </summary>
        /// <param name="hmiTarget">The hmi target.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">Parameter is null;hmiTarget</exception>
        /// <exception cref="System.ArgumentException">Parameter is null or empty;name</exception>
        public static Screen FindScreenByName(HmiTarget hmiTarget, string name)
        {
            if (hmiTarget == null)
                throw new ArgumentNullException(nameof(hmiTarget), "Parameter is null");
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Parameter is null or empty", nameof(name));

            return RecursiveFindElementByName(hmiTarget.ScreenFolder, name) as Screen;
        }

        /// <summary>
        /// Returns a reference to the HmiTagTable with given name if found, otherwise null.
        /// </summary>
        /// <param name="hmiTarget">The hmi target.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">Parameter is null;hmiTarget</exception>
        /// <exception cref="System.ArgumentException">Parameter is null or empty;name</exception>
        public static TagTable FindTagTableByName(HmiTarget hmiTarget, string name)
        {
            if (hmiTarget == null)
                throw new ArgumentNullException(nameof(hmiTarget), "Parameter is null");
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Parameter is null or empty", nameof(name));

            return RecursiveFindElementByName(hmiTarget.TagFolder, name) as TagTable;
        }

        #endregion

        #region GetAll

        /// <summary>Returns IEnumerable Collection of all screens in hmiTarget</summary>
        /// <param name="hmiTarget">The hmi target.</param>
        /// <returns>IEnumerable&lt;Screen&gt;</returns>
        /// <exception cref="System.ArgumentNullException">Parameter is null;hmiTarget</exception>
        public static IEnumerable<Screen> GetAllScreens(HmiTarget hmiTarget)
        {
            if (hmiTarget == null)
                throw new ArgumentNullException(nameof(hmiTarget), "Parameter is null");

            Collection<object> collection = new Collection<object>();

            RecursiveGetAllElements(hmiTarget.ScreenFolder, ref collection);

            return collection.Cast<Screen>();
        }

        /// <summary>Returns IEnumerable Collection of all TagTables in hmiTarget</summary>
        /// <param name="hmiTarget">The hmi target.</param>
        /// <returns>IEnumerable&lt;TagTable&gt;</returns>
        /// <exception cref="System.ArgumentNullException">Parameter is null;hmiTarget</exception>
        public static IEnumerable<TagTable> GetAllTagTables(HmiTarget hmiTarget)
        {
            if (hmiTarget == null)
                throw new ArgumentNullException(nameof(hmiTarget), "Parameter is null");

            Collection<object> collection = new Collection<object>();

            RecursiveGetAllElements(hmiTarget.TagFolder, ref collection);

            return collection.Cast<TagTable>();
        }

        /// <summary>Returns IEnumerable Collection of all TagTables in hmiTarget</summary>
        /// <param name="hmiTarget">The hmi target.</param>
        /// <returns>IEnumerable&lt;TagTable&gt;</returns>
        /// <exception cref="System.ArgumentNullException">Parameter is null;hmiTarget</exception>
        public static IEnumerable<Cycle> GetAllCycles(HmiTarget hmiTarget)
        {
            if (hmiTarget == null)
                throw new ArgumentNullException(nameof(hmiTarget), "Parameter is null");

            Collection<object> collection = new Collection<object>();

            RecursiveGetAllElements(hmiTarget.Cycles, ref collection);

            return collection.Cast<Cycle>();
        }

        /// <summary>Returns IEnumerable Collection of all TagTables in hmiTarget</summary>
        /// <param name="hmiTarget">The hmi target.</param>
        /// <returns>IEnumerable&lt;TagTable&gt;</returns>
        /// <exception cref="System.ArgumentNullException">Parameter is null;hmiTarget</exception>
        public static IEnumerable<Connection> GetAllConnections(HmiTarget hmiTarget)
        {
            if (hmiTarget == null)
                throw new ArgumentNullException(nameof(hmiTarget), "Parameter is null");

            Collection<object> collection = new Collection<object>();

            RecursiveGetAllElements(hmiTarget.Connections, ref collection);

            return collection.Cast<Connection>();
        }

        #endregion
    }
}
