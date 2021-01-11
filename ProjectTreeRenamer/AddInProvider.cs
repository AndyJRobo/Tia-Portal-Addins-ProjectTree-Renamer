﻿using Siemens.Engineering;
using Siemens.Engineering.AddIn;
using System.Collections.Generic;
using Siemens.Engineering.AddIn.Menu;

namespace ProjectTreeRenamer
{
    public sealed class AddInProvider : ProjectTreeAddInProvider
    {
        private readonly TiaPortal _tiaPortal;

        public AddInProvider(TiaPortal tiaPortal)
        {
            _tiaPortal = tiaPortal;
        }

        protected override IEnumerable<ContextMenuAddIn> GetContextMenuAddIns()
        {
            yield return new AddIn(_tiaPortal);
        }
    }
}
