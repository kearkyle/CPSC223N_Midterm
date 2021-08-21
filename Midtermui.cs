//Vong Chen
//223 midterm;

﻿using System;
using System.Drawing;
using System.Windows.Forms;
using System.Timers;

public class Midtermui : Form
{

    private Label author = new Label();
    private Label elapsed_time_label = new Label();
    private Label elapsed_time_legend = new Label();
    private Button startbutton = new Button();
    private Button pausebutton = new Button();
    private Button exitbutton = new Button();
    private Panel headerpanel = new Panel();
    private Rectangle display = new Rectangle(0,100,1024,500);
    
    private Panel controlpanel = new Panel();
    private double elapsed_time_accumulator = 0.0;
    private const double disc_speed = 90.0;
    private const double animation_clock_speed = 45.0;
    private const double delta = disc_speed / animation_clock_speed;
    private const double animation_clock_interval = 1000.0 / animation_clock_speed;
    int animation_clock_motion_speed_integer = (int)System.Math.Round(animation_clock_interval);
    private const double refresh_clock_speed = 24.0;
    private const double refresh_clock_interval = 1000/refresh_clock_speed;
    int refresh_clock_speed_integer = (int)System.Math.Round(refresh_clock_interval);

    //clocks
    private static System.Timers.Timer user_interface_refresh_clock = new System.Timers.Timer();
    private static System.Timers.Timer ball_update_clock = new System.Timers.Timer();
    //ui size
    private Size maximuminterfacesize = new Size(1024, 800);
    private Size minimuminterfacesize = new Size(1024, 800);

    private const double radius = 10;    //Radius measured in pixels
    private double x;  //This is x-coordinate of the upper left corner of the skateboard.
    private double y;  //This is y-coordinate of the upper left corner of the skateboard.

    public Midtermui()  //Constructor
    {
       Size = new Size(400,240); 
       MaximumSize = maximuminterfacesize;
       MinimumSize = minimuminterfacesize;
        //Initialize text strings
        Text = "Midterm animation";
        author.Text = "Midterm animation by Vong Chen";
        elapsed_time_label.Text = "Elapsed time (seconds):";
        elapsed_time_legend.Text = "000.00";
        startbutton.Text = "Start";
        pausebutton.Text = "Pause";
        exitbutton.Text = "Exit";

        //Set sizes
        author.Size = new Size(800, 44);
        elapsed_time_label.Size = new Size(200, 50);
        elapsed_time_legend.Size = new Size(100, 50);
        startbutton.Size = new Size(120, 60);
        pausebutton.Size = new Size(120, 60);
        exitbutton.Size = new Size(120, 60);
        headerpanel.Size = new Size(1024, 100);
        //displaypanel.Size = new Size(1024, 400);
        controlpanel.Size = new Size(1024, 200);

        //Set colors
        headerpanel.BackColor = Color.Blue;
        //displaypanel.BackColor = Color.BlueViolet;
        controlpanel.BackColor = Color.LightBlue;
        startbutton.BackColor = Color.Gray;
        pausebutton.BackColor = Color.Gray;
        exitbutton.BackColor = Color.Gray;
        elapsed_time_legend.BackColor = Color.White;
        elapsed_time_label.BackColor = Color.White;
        //Set fonts
        //welcome.Font = new Font("Times New Roman", 33, FontStyle.Bold);
        author.Font = new Font("Times New Roman", 26, FontStyle.Regular);
        //sequencemessage.Font = new Font("Arial", 26, FontStyle.Regular);
        elapsed_time_label.Font = new Font("Arial", 12, FontStyle.Regular);
        elapsed_time_legend.Font = new Font("Arial", 12, FontStyle.Regular);
        //outputinfo.Font = new Font("Arial", 26, FontStyle.Regular);
        startbutton.Font = new Font("Liberation Serif", 15, FontStyle.Regular);
        pausebutton.Font = new Font("Liberation Serif", 15, FontStyle.Regular);
        exitbutton.Font = new Font("Liberation Serif", 15, FontStyle.Regular);
        elapsed_time_label.TextAlign = ContentAlignment.MiddleCenter;
        elapsed_time_legend.TextAlign = ContentAlignment.MiddleCenter;


        //Set locations
        headerpanel.Location = new Point(0, 0);
        //welcome.Location = new Point(225, 26);
        author.Location = new Point(260, 40);
        elapsed_time_label.Location = new Point(420, 30);
        elapsed_time_legend.Location = new Point(620, 30);
        //outputinfo.Location = new Point(100, 200);
        startbutton.Location = new Point(50, 30);
        pausebutton.Location = new Point(200, 30);
        exitbutton.Location = new Point(820, 30);
        headerpanel.Location = new Point(0, 0);
        //displaypanel.Location = new Point(0, 200);
        controlpanel.Location = new Point(0, 570);

        //Associate the Compute button with the Enter key of the keyboard
        AcceptButton = startbutton;

        //Setting up the clocks
        user_interface_refresh_clock.Enabled = false;  //Initially this clock is stopped.
        user_interface_refresh_clock.Interval = refresh_clock_speed_integer;
        user_interface_refresh_clock.Elapsed += new ElapsedEventHandler(Refresh_user_interface);

       //Prepare the ball clock.  This clock is often referred to as the "animation clock".  A button will start this clock ticking.
        ball_update_clock.Enabled = false;
        ball_update_clock.Interval = animation_clock_motion_speed_integer;
        ball_update_clock.Elapsed += new ElapsedEventHandler(Update_ball_coordinates);
        //Add controls to  the form
        Controls.Add(headerpanel);
        headerpanel.Controls.Add(author);
        Controls.Add(controlpanel);
        controlpanel.Controls.Add(elapsed_time_label);
        controlpanel.Controls.Add(elapsed_time_legend);
        controlpanel.Controls.Add(startbutton);
        controlpanel.Controls.Add(pausebutton);
        controlpanel.Controls.Add(exitbutton);

        x = (double)650 - radius;
        y = (double)150 - radius;
        //Register the event handler.  In this case each button has an event handler, but no other 
        //controls have event handlers.
        startbutton.Click += new EventHandler(start);
        pausebutton.Click += new EventHandler(pause);
        exitbutton.Click += new EventHandler(stoprun);  //The '+' is required.

        //Open this user interface window in the center of the display.
        CenterToScreen();

    }//End of constructor

    Point A = new Point(650,150);
    Point B = new Point(260,400);
    Point C = new Point(700,400);
    protected override void OnPaint(PaintEventArgs ee) {
        Graphics graph = ee.Graphics;

        graph.FillRectangle(Brushes.Cornsilk,display); // displays background

        Point[] triangle = {A,B,C}; // creates triangle
        
        //graph.FillPolygon(Brushes.Cyan,triangle);
        graph.DrawLine(Pens.Black,A,B);
        graph.FillEllipse (Brushes.Gold,
                           (int)System.Math.Round(x),
                           (int)System.Math.Round(y),
                           (int)System.Math.Round(2.0*radius),
                           (int)System.Math.Round(2.0*radius));

    base.OnPaint(ee);
    } // End of Onpain
    //Point A = new Point(650,150);
    //Point B = new Point(260,400);
    //Point C = new Point(700,400);
    bool reverse = false;
    protected void Update_ball_coordinates(System.Object sender, ElapsedEventArgs even) {
        if (reverse == false){
            if(System.Math.Abs(x+radius-(double)60)>delta && System.Math.Abs(y+radius-(double)400)>delta) {
                x -= delta*(650-260)/(400-150)/1.5;
                y += delta/1.5;}
        else
            reverse = true;
        }
        if (reverse == true) {
            if (System.Math.Abs((double)650-x-radius) < 0.5 && System.Math.Abs((double)150-y-radius)<0.5)
                reverse = false;
            else {
            //if(System.Math.Abs((double)450-x-radius) > delta && System.Math.Abs((double)400-y-radius)>delta) {
                x-= -delta*(650-260)/(400-150)/1.5;
                y += -delta/1.5;}
               
        }
    elapsed_time_accumulator += (double)animation_clock_motion_speed_integer/1000.0;
    Invalidate();
    }// end of Update_ball_coordinates
    protected void Refresh_user_interface(System.Object sender, ElapsedEventArgs even)  //See Footnote #2
    {elapsed_time_legend.Text = String.Format("{0:000.00}",elapsed_time_accumulator);
        Invalidate();
    }//End of event handler Refresh_user_interface
    protected void start(Object sender, EventArgs events) {
        System.Console.WriteLine("The animation has started.");
        user_interface_refresh_clock.Enabled = true;
        ball_update_clock.Enabled = true;
        startbutton.Enabled = false;
        pausebutton.Enabled = true;
        Invalidate();
    } // End of start

 // pause the traffic light
    protected void pause(Object sender, EventArgs events) {
            System.Console.WriteLine("The animation has been paused.");
            user_interface_refresh_clock.Enabled = false;
            ball_update_clock.Enabled = false;
            pausebutton.Enabled = false;
            startbutton.Enabled = true;
        Invalidate();
    }
    
    //Method to execute when the exit button receives an event, namely: receives a mouse click
    protected void stoprun(Object sender, EventArgs events)
    {
        Close();
    }//End of stoprun

}//End of class