// -----------------------------------------------------------------------
// <copyright file="MapView.cs" company="Conglomo">
// Copyright 2015 Peter Chapman. Please see LICENCE.md for licence details.
// </copyright>
// -----------------------------------------------------------------------

namespace Conglomo.NZWalks
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
#if WINDOWS_APP
    using Bing.Maps;
#endif
    using Windows.Devices.Geolocation;
    using Windows.UI.Xaml.Controls;
#if WINDOWS_PHONE_APP
    using Windows.UI.Xaml.Controls.Maps;
#endif

    /// <summary>
    /// A map view control.
    /// </summary>
    public class MapView : Grid, INotifyPropertyChanged, IDisposable
    {
        /// <summary>
        /// Tracks whether Dispose has been called.
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// The map.
        /// </summary>
#if WINDOWS_APP
        private Map map;
#elif WINDOWS_PHONE_APP
        private MapControl map;
#endif

#if WINDOWS_APP
        /// <summary>
        /// The map shape layer.
        /// </summary>
        private MapShapeLayer shapeLayer;
#endif
        /// <summary>
        /// Initialises a new instance of the <see cref="MapView" /> class.
        /// </summary>
        public MapView()
        {
#if WINDOWS_APP
            this.map = new Map();
            this.shapeLayer = new MapShapeLayer();
            this.map.ShapeLayers.Add(this.shapeLayer);
#elif WINDOWS_PHONE_APP
            this.map = new MapControl();
#endif

            this.Children.Add(this.map);
        }

        /// <summary>
        /// Finalises an instance of the <see cref="MapView"/> class.
        /// </summary>
        ~MapView()
        {
            // Do not re-create Dispose clean-up code here.
            // Calling Dispose(false) is optimal in terms of
            // readability and maintainability.
            this.Dispose(false);
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the centre.
        /// </summary>
        /// <value>
        /// The centre.
        /// </value>
        public Geopoint Center
        {
            get
            {
#if WINDOWS_APP
                return this.map.Center.ToGeopoint();
#elif WINDOWS_PHONE_APP
                return this.map.Center;
#endif
            }

            set
            {
#if WINDOWS_APP
                this.map.Center = value.ToLocation();
#elif WINDOWS_PHONE_APP
                this.map.Center = value;
#endif
                this.OnPropertyChanged("Center");
            }
        }

        /// <summary>
        /// Gets or sets the credentials.
        /// </summary>
        /// <value>
        /// The credentials.
        /// </value>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", Justification = "This is not required for Windows Phone")]
        public string Credentials
        {
            get
            {
#if WINDOWS_APP
                return this.map.Credentials;
#elif WINDOWS_PHONE_APP
                return string.Empty;
#endif
            }

            set
            {
#if WINDOWS_APP
                if (!string.IsNullOrEmpty(value))
                {
                    this.map.Credentials = value;
                }
#endif
                this.OnPropertyChanged("Credentials");
            }
        }

        /// <summary>
        /// Gets or sets the map service token.
        /// </summary>
        /// <value>
        /// The map service token.
        /// </value>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", Justification = "This is not required for Windows Phone")]
        public string MapServiceToken
        {
            get
            {
#if WINDOWS_APP
                return string.Empty;
#elif WINDOWS_PHONE_APP
                return this.map.MapServiceToken;
#endif
            }

            set
            {
#if WINDOWS_PHONE_APP
                if (!string.IsNullOrEmpty(value))
                {
                    this.map.MapServiceToken = value;
                }
#endif
                this.OnPropertyChanged("MapServiceToken");
            }
        }

        /// <summary>
        /// Gets or sets the zoom.
        /// </summary>
        /// <value>
        /// The zoom.
        /// </value>
        public double Zoom
        {
            get
            {
                return this.map.ZoomLevel;
            }

            set
            {
                this.map.ZoomLevel = value;
                this.OnPropertyChanged("Zoom");
            }
        }

        /// <summary>
        /// Adds the DOC tracks to the map.
        /// </summary>
        public void AddDocTracks()
        {
#if WINDOWS_APP
            // Create a new map layer to add the tile overlay to
            MapTileLayer tileLayer = new MapTileLayer();

            // Add the tile overlay to the map layer
            tileLayer.TileSource = "https://nzwalks.azurewebsites.net/geoserver/gwc/service/ve?quadkey={quadkey}&format=image/png&layers=doc_tracks:doc-tracks&srs=EPSG:3857";

            // Display the layer
            tileLayer.Opacity = 1;

            // Add the map layer to the map
            this.map.TileLayers.Add(tileLayer);
#elif WINDOWS_PHONE_APP
            // Get the tile overlay
            HttpMapTileDataSource httpSource = new HttpMapTileDataSource("https://nzwalks.azurewebsites.net/geoserver/gwc/service/ve?quadkey={quadkey}&format=image/png&layers=doc_tracks:doc-tracks&srs=EPSG:3857");

            // Add the tile overlay to the map layer
            MapTileSource tileSource = new MapTileSource(httpSource);

            // Add the map layer to the map
            this.map.TileSources.Add(tileSource);
#endif
        }

        /// <summary>
        /// Disposes of the current object.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);

            // This object will be cleaned up by the Dispose method. 
            // Therefore, you should call GC.SupressFinalize to 
            // take this object off the finalization queue 
            // and prevent finalization code for this object 
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Called when a property is changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        internal void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != default(PropertyChangedEventHandler) && !string.IsNullOrEmpty(propertyName))
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Disposes the current instance.
        /// </summary>
        /// <param name="disposing">If <c>true</c>, dispose all managed and unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed and unmanaged resources. 
                if (disposing)
                {
                    // Dispose managed resources.
#if WINDOWS_APP
                    this.map.Dispose();
#endif
                }

                // Note disposing has been done.
                this.disposed = true;
            }
        }
    }
}
