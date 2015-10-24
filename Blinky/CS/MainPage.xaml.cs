// Copyright (c) Microsoft. All rights reserved.

using System;
using Windows.Devices.Gpio;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;

namespace Blinky
{
    public sealed partial class MainPage : Page
    {
        private const int LED_PIN = 5;
        private int SPEED = 250;
        private GpioPin pin;
        private GpioPinValue pinValue;
        private DispatcherTimer timer;
        private SolidColorBrush redBrush = new SolidColorBrush(Windows.UI.Colors.Red);
        private SolidColorBrush grayBrush = new SolidColorBrush(Windows.UI.Colors.LightGray);

        public MainPage()
        {
            InitializeComponent();
            DelayText.Text = SPEED + "ms";
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(SPEED);
            timer.Tick += Timer_Tick;
            InitGPIO();
            if (pin != null)
            {
                timer.Start();
            }        
        }

        private void InitGPIO()
        {
            var gpio = GpioController.GetDefault();

            // Show an error if there is no GPIO controller
            if (gpio == null)
            {
                pin = null;
                GpioStatus.Text = "There is no GPIO controller on this device.";
                return;
            }

            pin = gpio.OpenPin(LED_PIN);
            pinValue = GpioPinValue.High;
            pin.Write(pinValue);
            pin.SetDriveMode(GpioPinDriveMode.Output);

            GpioStatus.Text = "GPIO pin initialized correctly.";

        }
        private void Update()
        {
            DelayText.Text = SPEED + "ms";
            timer.Interval = TimeSpan.FromMilliseconds(SPEED);

        }
        private void Timer_Tick(object sender, object e)
        {
            if (pinValue == GpioPinValue.High)
            {
                pinValue = GpioPinValue.Low;
                pin.Write(pinValue);
                LED.Fill = redBrush;
            }
            else
            {
                pinValue = GpioPinValue.High;
                pin.Write(pinValue);
                LED.Fill = grayBrush;
            }
        }

        private void DelayText_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void BtnDecrease_Click(object sender, RoutedEventArgs e)
        {
            if (SPEED >= 20)
            {
                SPEED -= 10;
            }
            Update();
        }

        private void BtnIncrease_Click(object sender, RoutedEventArgs e)
        {
            if (SPEED <= 740)
            {
                SPEED += 10;
            }
            Update();
        }
    }
}
