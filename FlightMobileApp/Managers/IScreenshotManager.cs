using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightMobileApp.Managers
{
    public interface IScreenshotManager
    {
        public Task<Byte[]> GetScreenshot();
    }
}
