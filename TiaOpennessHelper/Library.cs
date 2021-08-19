using Siemens.Engineering.Library;
using Siemens.Engineering.Library.MasterCopies;
using Siemens.Engineering.Library.Types;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace TiaOpennessHelper
{
    public partial class OpennessHelper
    {
        /// <summary>Updates all LibraryTypes from globalLib in target</summary>
        /// <param name="target">The target.</param>
        /// <param name="library">The library.</param>
        /// <exception cref="System.ArgumentNullException">
        /// Argument is null;target
        /// or
        /// Argument is null;library
        /// </exception>
        /// <exception cref="System.ArgumentException"></exception>
        /// <exception cref="System.IO.IOException"></exception>
        /// <exception cref="System.UnauthorizedAccessException"></exception>
        /// <exception cref="System.IO.DirectoryNotFoundException"></exception>
        public static void UpdateAllTypesInTarget(IUpdateProjectScope target, ILibrary library)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target), "Parameter is null");
            if (library == null)
                throw new ArgumentNullException(nameof(library), "Parameter is null");

            IEnumerable<LibraryType> col = GetAllTypes(library);
            foreach (LibraryType type in col)
            {
                type.UpdateProject(target);
            }

        }

        /// <summary>
        /// Returns IEnumerable Collection of all LibraryTypes in library
        /// </summary>
        /// <param name="library">The library.</param>
        /// <returns>IEnumerable&lt;LibraryType&gt;</returns>
        /// <exception cref="System.ArgumentNullException">Parameter is null;library</exception>
        public static IEnumerable<LibraryType> GetAllTypes(ILibrary library)
        {
            if (library == null)
                throw new ArgumentNullException(nameof(library), "Parameter is null");

            Collection<object> col = new Collection<object>();
            RecursiveGetAllElements(library.TypeFolder, ref col);

            return col.Cast<LibraryType>();
        }

        /// <summary>Returns the LibraryType with given name</summary>
        /// <param name="library">The library.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">Parameter is null;library</exception>
        /// <exception cref="System.ArgumentException">Parameter is null or empty;name</exception>
        public static LibraryType FindTypeByName(ILibrary library, string name)
        {
            if (library == null)
                throw new ArgumentNullException(nameof(library), "Parameter is null");
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Parameter is null or empty", nameof(name));

            return RecursiveFindElementByName(library.TypeFolder, name) as LibraryType;
        }

        /// <summary>Returns the MasterCopy with given name</summary>
        /// <param name="library">The library.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">Parameter is null;library</exception>
        /// <exception cref="System.ArgumentException">Parameter is null or empty;name</exception>
        public static MasterCopy FindMasterCopyByName(ILibrary library, string name)
        {
            if (library == null)
                throw new ArgumentNullException(nameof(library), "Parameter is null");
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Parameter is null or empty", nameof(name));

            return RecursiveFindElementByName(library.MasterCopyFolder, name) as MasterCopy;
        }
    }
}
