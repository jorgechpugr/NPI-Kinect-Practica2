namespace KinectCam
{
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Threading;
    using KinectStatusNotifier;
    using Microsoft.Kinect;
    using System.Linq;
    using System.Windows.Shapes;
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Kinect Sensor Instance
        /// </summary>
        private KinectSensor sensor;

        /// <summary>
        /// Kinect Status Notifer
        /// </summary>
        private StatusNotifier statusNotifier = new StatusNotifier();

        /// <summary>
        /// View Model 
        /// </summary>
        private MainWindowViewModel viewModel;

        /// <summary>
        /// Gets or sets the pixel data.
        /// </summary>
        /// <value>The pixel data.</value>
        private byte[] pixelData;


        private EjercicioMacarena ejercicio;


        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow" /> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
            this.viewModel = new MainWindowViewModel();
            this.DataContext = this.viewModel;
            this.Loaded += this.MainWindow_Loaded;
            this.ejercicio = new EjercicioMacarena();
            this.viewModel.CanStart = true;
        }



        /// <summary>
        /// Handles the Loaded event of the MainWindow control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        /// <exception cref="System.NotImplementedException">Not Implemented Exception</exception>
        protected void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.StartKinectCam();
        }

        /// <summary>
        /// Handles the ColorFrameReady event of the sensor control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ColorImageFrameReadyEventArgs" /> instance containing the event data.</param>
        protected void sensor_ColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            using (ColorImageFrame imageFrame = e.OpenColorImageFrame())
            {
                // Check if the incoming frame is not null
                if (imageFrame == null)
                {
                    return;
                }
                else
                {
                    // Get the pixel data in byte array
                    this.pixelData = new byte[imageFrame.PixelDataLength];

                    // Copy the pixel data
                    imageFrame.CopyPixelDataTo(this.pixelData);

                    this.VideoControl.Source = BitmapSource.Create(imageFrame.Width, imageFrame.Height, 96, 96, PixelFormats.Bgr32, null, this.pixelData, imageFrame.Width * 4);

                }
            }
        }

        /// <summary>
        /// Starts the kinect cam.
        /// </summary>
        private void StartKinectCam()
        {
            if (KinectSensor.KinectSensors.Count > 0)
            {
                this.sensor = KinectSensor.KinectSensors.FirstOrDefault(sensorItem => sensorItem.Status == KinectStatus.Connected);
                this.statusNotifier.Sensors = KinectSensor.KinectSensors;
                //this.StartSensor();
                this.sensor.ColorStream.Enable();
                this.sensor.ColorFrameReady += this.sensor_ColorFrameReady;

                // Turn on the skeleton stream to receive skeleton frames
                this.sensor.SkeletonStream.Enable();

                // Add an event handler to be called whenever there is new color frame data
                this.sensor.SkeletonFrameReady += this.SensorSkeletonFrameReady;
            }
            else
            {
                MessageBox.Show("No device is connected with system!");
                this.Close();
            }
        }

        
        /// <summary>
        /// Event handler for Kinect sensor's SkeletonFrameReady event
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void SensorSkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            Skeleton[] skeletons = new Skeleton[0];

            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
            {
                if (skeletonFrame != null)
                {
                    skeletons = new Skeleton[skeletonFrame.SkeletonArrayLength];
                    skeletonFrame.CopySkeletonDataTo(skeletons);

                    Skeleton skel = new Skeleton();

                    if (skeletons.Length != 0)
                    {
                        foreach (Skeleton skele in skeletons)
                        {
                            if (skele.TrackingState == SkeletonTrackingState.Tracked)
                            {
                                skel = skele;
                            }
                        }
                    }

                    if (skel.TrackingState == SkeletonTrackingState.Tracked)
                    {
                        int pos = ejercicio.CompruebaMovimiento(skel);

                        if (pos == 0)
                        {
                          

                            //this.Joints(skel);
                            
                        }
                        else if (pos == 1)
                        {
                            this.borPos1.Background = Brushes.Green;
                            //Brush color = Brushes.Red;
                            //this.Joints(skel, color);
                        }
                        else if (pos == 2)
                        {
                            this.borPos1.Background = Brushes.Purple;
                            this.borPos2.Background = Brushes.Green;
                        }
                        else if (pos == 3)
                        {
                            this.borPos2.Background = Brushes.Purple;
                            this.borPos3.Background = Brushes.Green;
                        }
                        else if (pos == 4)
                        {
                            this.borPos3.Background = Brushes.Purple;
                            this.borPos4.Background = Brushes.Green;
                        }
                        else if (pos == 5)
                        {
                            this.borPos4.Background = Brushes.Purple;
                            this.borPos5.Background = Brushes.Green;
                        }
                        else if (pos == 6)
                        {
                            this.borPos5.Background = Brushes.Purple;
                            this.borPos6.Background = Brushes.Green;
                        }
                        else if (pos == 7)
                        {
                            this.borPos6.Background = Brushes.Purple;
                            this.borPos7.Background = Brushes.Green;
                        }
                        else if (pos == 8)
                        {
                            this.borPos7.Background = Brushes.Purple;
                            this.borPos8.Background = Brushes.Green;
                        }
                        if (pos == 9)
                        {
                            this.borPos1.Background = Brushes.Purple;
                            this.borPos2.Background = Brushes.Purple;
                            this.borPos3.Background = Brushes.Purple;
                            this.borPos4.Background = Brushes.Purple;
                            this.borPos5.Background = Brushes.Purple;
                            this.borPos6.Background = Brushes.Purple;
                            this.borPos7.Background = Brushes.Purple;
                            this.borPos8.Background = Brushes.Purple;
                        }
                        else if (pos == -1)
                        {
                            this.borPos1.Background = Brushes.Red;
                            this.borPos2.Background = Brushes.Red;
                            this.borPos3.Background = Brushes.Red;
                            this.borPos4.Background = Brushes.Red;
                            this.borPos5.Background = Brushes.Red;
                            this.borPos6.Background = Brushes.Red;
                            this.borPos7.Background = Brushes.Red;
                            this.borPos8.Background = Brushes.Red;
                        }
                    }
                }
            }
        }

        /*

        private void SensorSkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            Skeleton[] skeletons = new Skeleton[0];

            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
            {
                if (skeletonFrame != null)
                {
                    skeletons = new Skeleton[skeletonFrame.SkeletonArrayLength];
                    skeletonFrame.CopySkeletonDataTo(skeletons);
                }
            }

            
            if (skeletons.Length != 0)
            {
                foreach (Skeleton skel in skeletons)
                {
                    //RenderClippedEdges(skel, dc);

                    if (skel.TrackingState == SkeletonTrackingState.Tracked)
                    {
                        this.Joints(skel);
                    }
                }
            }

        }
        */
        private void DibujaElipse(float x, float y, float z)
        {
            SkeletonPoint point = new SkeletonPoint();
            point.X = x;
            point.Y = y;
            point.Z = z;

            Point ell = this.SkeletonPointToScreen(point);
            Ellipse aux = new Ellipse();
            aux.Width = 20;
            aux.Height = 20;
            aux.Fill = Brushes.Orange;
            Canvas.SetTop(aux, ell.Y);
            Canvas.SetLeft(aux, ell.X);
            this.SkeletonControl.Children.Add(aux);
        }

        private void DibujaElipse(Joint joint)
        {
            Point ell = this.SkeletonPointToScreen(joint.Position);
            Ellipse aux = new Ellipse();
            aux.Width = 20;
            aux.Height = 20;
            aux.Fill = Brushes.Orange;
            Canvas.SetTop(aux, ell.Y);
            Canvas.SetLeft(aux, ell.X);
            this.SkeletonControl.Children.Add(aux);
        }

        private void DibujaElipse(Joint joint, Brush color)
        {
            Point ell = this.SkeletonPointToScreen(joint.Position);
            Ellipse aux = new Ellipse();
            aux.Width = 20;
            aux.Height = 20;
            aux.Fill = color;
            Canvas.SetTop(aux, ell.Y);
            Canvas.SetLeft(aux, ell.X);
            this.SkeletonControl.Children.Add(aux);
        }


        private Point SkeletonPointToScreen(SkeletonPoint skelpoint)
        {
            // Convert point to depth space.  
            // We are not using depth directly, but we do want the points in our 640x480 output resolution.
            DepthImagePoint depthPoint = this.sensor.CoordinateMapper.MapSkeletonPointToDepthPoint(skelpoint, DepthImageFormat.Resolution640x480Fps30);
            return new Point(depthPoint.X, depthPoint.Y);
        }
        
        private void Joints(Skeleton skeleton)
        {
            if (this.SkeletonControl.Children.Count > 20)
                this.SkeletonControl.Children.Clear();
            // Render Joints
            foreach (Joint joint in skeleton.Joints)
            {
                Brush drawBrush = null;

                if (joint.TrackingState == JointTrackingState.Tracked)
                {
                    drawBrush = Brushes.Orange;
                }
                else if (joint.TrackingState == JointTrackingState.Inferred)
                {
                    drawBrush = Brushes.Purple;
                }

                if (drawBrush != null)
                {
                    DibujaElipse(joint);
                }
            }
        }

        private void Joints(Skeleton skeleton, Brush color)
        {
            if (this.SkeletonControl.Children.Count > 20)
                this.SkeletonControl.Children.Clear();
            // Render Joints
            foreach (Joint joint in skeleton.Joints)
            {
                Brush drawBrush = null;

                if (joint.TrackingState == JointTrackingState.Tracked)
                {
                    drawBrush = Brushes.Orange;
                }
                else if (joint.TrackingState == JointTrackingState.Inferred)
                {
                    drawBrush = Brushes.Purple;
                }

                if (drawBrush != null)
                {
                    DibujaElipse(joint,color);
                }
            }
        }
        
        /// <summary>
        /// Handles the Click event of the ButtonStart control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void ButtonStart_Click(object sender, RoutedEventArgs e)
        {
            this.StartSensor();
            this.borPos1.Background = Brushes.Red;
            this.borPos2.Background = Brushes.Red;
            this.borPos3.Background = Brushes.Red;
            this.borPos4.Background = Brushes.Red;
            this.borPos5.Background = Brushes.Red;
            this.borPos6.Background = Brushes.Red;
            this.borPos7.Background = Brushes.Red;
            this.borPos8.Background = Brushes.Red;
            /*System.Media.SoundPlayer sp = new System.Media.SoundPlayer("/KinectCam;component/song/Los Del Rio La Macarena Version Original Espaol (mp3cut.net).wav");
            sp.Play();*/
        }

        /// <summary>
        /// Handles the Click event of the ButtonStop control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void ButtonStop_Click(object sender, RoutedEventArgs e)
        {
            this.ejercicio.Reset();
            this.StopSensor();
        }

        /// <summary>
        /// Handles the Click event of the ButtonExit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            this.StopSensor();
            this.Close();
        }

        /// <summary>
        /// Starts the sensor.
        /// </summary>
        private void StartSensor()
        {
            if (this.sensor != null && !this.sensor.IsRunning)
            {
                this.sensor.Start();
                this.viewModel.CanStart = false;
                this.viewModel.CanStop = true;
            }
        }

        /// <summary>
        /// Stops the sensor.
        /// </summary>
        private void StopSensor()
        {
            if (this.sensor != null && this.sensor.IsRunning)
            {
                this.sensor.Stop();
                this.viewModel.CanStart = true;
                this.viewModel.CanStop = false;
            }
        }

    }
}
