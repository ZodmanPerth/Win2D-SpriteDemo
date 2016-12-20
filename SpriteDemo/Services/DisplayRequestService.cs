using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System.Display;

namespace SpriteDemo.Services
{
    /// <summary>
    /// Keeps the screen alive while the app is in the foreground
    /// </summary>
    public class DisplayRequestService
    {
        DisplayRequest _displayRequest;

        public void Activate()
        {
            if (_displayRequest == null) _displayRequest = new DisplayRequest();
            _displayRequest.RequestActive();
        }

        public void DeActivate()
        {
            if (_displayRequest == null) return;
            _displayRequest.RequestRelease();
        }
    }
}
