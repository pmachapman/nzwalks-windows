// -----------------------------------------------------------------------
// <copyright file="MainPage.xaml.cs" company="Conglomo">
// Copyright 2015 Peter Chapman. Please see LICENCE.md for licence details.
// </copyright>
// -----------------------------------------------------------------------

namespace Conglomo.NZWalks
{
    using System;
    using Windows.Devices.Geolocation;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Navigation;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// <remarks>
    /// The Blank Page item template is documented at <c>http://go.microsoft.com/fwlink/?LinkId=234238</c>.
    /// </remarks>
    public sealed partial class MainPage : Page
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="MainPage"/> class.
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;
            this.Loaded += this.MainPage_Loaded;

            // Set up the credentials
            this.MainMap.Credentials = "YOUR_CREDENTIALS_HERE";
            this.MainMap.MapServiceToken = "YOUR_MAP_SERVICE_TOKEN_HERE";
        }

        /// <summary>
        /// Handles the Loaded event of the MainPage.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            // Set up the default position
            BasicGeoposition position = new BasicGeoposition() { Latitude = -39.2833, Longitude = 175.5667 };

            // Get the GPS position
            try
            {
                Geolocator geolocator = new Geolocator();
                Geoposition geoposition = await geolocator.GetGeopositionAsync();
                if (geoposition != default(Geoposition))
                {
                    Geocoordinate geocoordinate = geoposition.Coordinate;
                    if (geocoordinate != default(Geocoordinate))
                    {
                        // Make sure the position is in New Zealand
                        if ((geocoordinate.Point.Position.Longitude > 160 || geocoordinate.Point.Position.Longitude < -172) && geocoordinate.Point.Position.Latitude > -53 && geocoordinate.Point.Position.Latitude < -30)
                        {
                            position = new BasicGeoposition() { Latitude = geocoordinate.Point.Position.Latitude, Longitude = geocoordinate.Point.Position.Longitude };
                        }
                    }
                }
            }
            catch (Exception)
            {
                // User blocked location, or another error of that sort
            }

            // Set the map center
            this.MainMap.Center = new Geopoint(position);
            this.MainMap.Zoom = 10;
            this.MainMap.AddDocTracks();
        }
    }
}
