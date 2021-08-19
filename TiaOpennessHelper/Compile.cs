using Siemens.Engineering;
using Siemens.Engineering.Compiler;
using System;

namespace TiaOpennessHelper
{
    public partial class OpennessHelper
    {
        #region Public Methods

        /// <summary>Compiles elementToCompile</summary>
        /// <param name="elementToCompile">The element to compile.</param>
        /// <returns>CompilerResult</returns>
        public static CompilerResult CompileObject(IEngineeringServiceProvider elementToCompile)
        {
            if (elementToCompile == null)
            {
                throw new ArgumentNullException(nameof(elementToCompile), "Parameter is null");
            }

            CompilerResult result;

            ICompilable compiler = elementToCompile.GetService<ICompilable>();
            if (compiler != null)
                result = compiler.Compile();
            else
                throw new ArgumentException("Parameter cannot be compiled.", nameof(elementToCompile));

            return result;
        }

        #endregion
    }
}