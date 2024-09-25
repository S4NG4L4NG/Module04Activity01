namespace Mod4
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
        }

       private async void OnGetLocation(object sender, EventArgs e)
        {
            try
            {
                var location = await Geolocation.GetLocationAsync();
                if (location == null)
                {
                    location = await Geolocation.GetLocationAsync(new GeolocationRequest
                    {
                        DesiredAccuracy = GeolocationAccuracy.High
                    });
                }

                if (location != null)
                {
                    LocationLabel.Text = $"Latitude: {location.Latitude}, Longtitude: {location.Longitude}";

                    //get address from Lat and Long

                    var placemarks = await Geocoding.Default.GetPlacemarksAsync(location.Latitude, location.Longitude);

                    var placemark = placemarks?.FirstOrDefault();

                    if (placemark != null)
                    {
                        AddressLabel.Text = $"Address: {placemark.Thoroughfare}," +
                            $"{placemark.Locality}," +
                            $"{placemark.AdminArea}," +
                            $"{placemark.PostalCode}, " +
                            $"{placemark.CountryName}";
                    }
                    else
                    {
                        AddressLabel.Text = "Unable to get address";
                    }
                }

                else
                {
                    LocationLabel.Text = "Unable to locate the location";
                }
            }

            catch (Exception ex)
            {
                LocationLabel.Text = $"Error: {ex.Message}";
            }
        }

        private async void OnGetCamera(object sender, EventArgs e)
        {
            try
            {
                if (MediaPicker.Default.IsCaptureSupported)
                {
                    FileResult photo = await MediaPicker.Default.CapturePhotoAsync();
                    if (photo != null)
                    {
                        await LoadPhotoAsync(photo);
                    }
                }
                else
                {

                }
            }

            catch (Exception ex)
            {
                await DisplayAlert("Error", $"cant open cameraaaghhh: {ex.Message}", "Sure");
            }

        }

            private async Task LoadPhotoAsync(FileResult photo)
            {

                if (photo == null)
                return;

                Stream stream = await photo.OpenReadAsync();

                CapturedImage.Source = ImageSource.FromStream(()=>stream);


            }

        
   
    }

}
