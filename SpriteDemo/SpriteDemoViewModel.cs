using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpriteDemo.Extensions;
using System.Collections.ObjectModel;

namespace SpriteDemo
{
    public class SpriteDemoViewModel : INotifyPropertyChanged
    {
        public double DeltaTheta { get; private set; }
        public double ThetaStep { get; private set; }
        public float a { get; private set; }
        public float b { get; private set; }
        public float c { get; private set; }
        public float m { get; private set; }
        public float n { get; private set; }
        public float o { get; private set; }

        #region DeltaThetaInteger

        int _deltaThetaDegrees;

        /// <summary>
        /// The change in Theta as an integer to overcome bug in slider
        /// </summary>
        public int DeltaThetaInteger
        {
            get { return _deltaThetaDegrees; }
            set
            {
                if (Equals(_deltaThetaDegrees, value)) { return; }
                _deltaThetaDegrees = value;
                DeltaTheta = _deltaThetaDegrees * 0.001;
                RaisePropertyChanged("DeltaThetaInteger");
                RaisePropertyChanged("DeltaTheta");
            }
        }

        #endregion

        #region SpriteCount

        int _spriteCount;

        /// <summary>
        /// The number of sprites
        /// </summary>
        public int SpriteCount
        {
            get { return _spriteCount; }
            set
            {
                if (Equals(_spriteCount, value)) { return; }
                _spriteCount = value;
                ThetaStep = GeometryHelpers.TwoPi / _spriteCount;
                RaisePropertyChanged("SpriteCount");
                RaisePropertyChanged("ThetaStep");
            }
        }

        #endregion

        #region aInteger

        int _aInteger;

        public int aInteger
        {
            get
            {
                return _aInteger;
            }
            set
            {
                if (object.Equals(_aInteger, value)) { return; }
                _aInteger = value;
                a = _aInteger;
                RaisePropertyChanged("aInteger");
                RaisePropertyChanged("a");
            }
        }

        #endregion

        #region bInteger

        int _bInteger;

        public int bInteger
        {
            get
            {
                return _bInteger;
            }
            set
            {
                if (object.Equals(_bInteger, value)) { return; }
                _bInteger = value;
                b = _bInteger * 0.01f;
                RaisePropertyChanged("bInteger");
                RaisePropertyChanged("b");
            }
        }

        #endregion

        #region cInteger

        int _cInteger;

        public int cInteger
        {
            get
            {
                return _cInteger;
            }
            set
            {
                if (object.Equals(_cInteger, value)) { return; }
                _cInteger = value;
                c = _cInteger * 0.001f;
                RaisePropertyChanged("cInteger");
                RaisePropertyChanged("c");
            }
        }

        #endregion

        #region mInteger

        int _mInteger;

        public int mInteger
        {
            get
            {
                return _mInteger;
            }
            set
            {
                if (object.Equals(_mInteger, value)) { return; }
                _mInteger = value;
                m = _mInteger;
                RaisePropertyChanged("mInteger");
                RaisePropertyChanged("m");
            }
        }

        #endregion

        #region nInteger

        int _nInteger;

        public int nInteger
        {
            get
            {
                return _nInteger;
            }
            set
            {
                if (object.Equals(_nInteger, value)) { return; }
                _nInteger = value;
                n = _nInteger * 0.01f;
                RaisePropertyChanged("nInteger");
                RaisePropertyChanged("n");
            }
        }

        #endregion

        #region oInteger

        int _oInteger;

        public int oInteger
        {
            get
            {
                return _oInteger;
            }
            set
            {
                if (object.Equals(_oInteger, value)) { return; }
                _oInteger = value;
                o = _oInteger * 0.001f;
                RaisePropertyChanged("oInteger");
                RaisePropertyChanged("o");
            }
        }

        #endregion

        #region ShowDebug

        bool _showDebug;

        /// <summary>
        /// Show all the debug info
        /// </summary>
        public bool ShowDebug
        {
            get { return _showDebug; }
            set
            {
                if (object.Equals(_showDebug, value)) { return; }
                _showDebug = value;
                RaisePropertyChanged("ShowDebug");
            }
        }

        #endregion

        #region DrawModesAvailable

        ObservableCollection<DrawModes> _drawModesAvailable;

        public ObservableCollection<DrawModes> DrawModesAvailable
        {
            get { return _drawModesAvailable; }
            set
            {
                if (Equals(_drawModesAvailable, value)) { return; }
                _drawModesAvailable = value;
                RaisePropertyChanged("DrawModesAvailable");
            }
        }

        #endregion


        #region DrawMode

        DrawModes _drawMode;

        /// <summary>
        /// The selected draw mode
        /// </summary>
        public DrawModes DrawMode
        {
            get { return _drawMode; }
            set
            {
                if (object.Equals(_drawMode, value)) { return; }
                _drawMode = value;
                RaisePropertyChanged("DrawMode");
            }
        }

        #endregion

        #region Construction

        public SpriteDemoViewModel()
        {
            _drawModesAvailable = new ObservableCollection<DrawModes>();
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Method to raise a property change event on a property
        /// </summary>
        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
