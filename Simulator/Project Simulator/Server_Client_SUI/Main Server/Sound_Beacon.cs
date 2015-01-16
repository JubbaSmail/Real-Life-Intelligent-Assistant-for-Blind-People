using System;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using System.Drawing;
using System.Media;
using System.Threading;

namespace Main_Server
{
    public class c_Sound_Beacon
    {
        static SoundEffect SE_forward = SoundEffect.FromStream(TitleContainer.OpenStream("Data/Forward.wav"));
        static SoundEffect SE_backward = SoundEffect.FromStream(TitleContainer.OpenStream("Data/Backword.wav"));
        static SoundEffect SE_left = SoundEffect.FromStream(TitleContainer.OpenStream("Data/Left.wav"));
        static SoundEffect SE_right = SoundEffect.FromStream(TitleContainer.OpenStream("Data/Right.wav"));
        static SoundEffect SE_forwardRight = SoundEffect.FromStream(TitleContainer.OpenStream("Data/Forward_Right.wav"));
        static SoundEffect SE_forwardLeft = SoundEffect.FromStream(TitleContainer.OpenStream("Data/Forward_Left.wav"));
        static SoundEffect SE_backwardRight = SoundEffect.FromStream(TitleContainer.OpenStream("Data/Backword_Right.wav"));
        static SoundEffect SE_backwardLeft = SoundEffect.FromStream(TitleContainer.OpenStream("Data/Backword_Left.wav"));

        SoundEffectInstance forward = SE_forward.CreateInstance();
        SoundEffectInstance backward = SE_backward.CreateInstance();
        SoundEffectInstance left = SE_left.CreateInstance();
        SoundEffectInstance right = SE_right.CreateInstance();
        SoundEffectInstance forwardRight = SE_forwardRight.CreateInstance();
        SoundEffectInstance forwardLeft = SE_forwardLeft.CreateInstance();
        SoundEffectInstance backwardRight = SE_backwardRight.CreateInstance();
        SoundEffectInstance backwardLeft = SE_backwardLeft.CreateInstance();
        public static bool manual = false;
        public static short[] manula_data = new short[2]; 
        public short[] m_Active_Sound_Beacon(bool open,System.Drawing.Point p1, System.Drawing.Point p2, System.Drawing.Point p3)
        {
            if (!open)
                return new short[2] { -2, -2 };
            short[] resultArray = new short[2];
            float distance = (float)Math.Sqrt(Math.Pow(p3.X - p2.X, 2) + Math.Pow(p3.Y - p2.Y, 2)) / 100;  // 100 for normalization
            resultArray[0] = (short)Math.Round(distance);
            double LightHouseAngle = 0;
            string result = m_Get_Direction_and_Degree(p1, p2, p3);
            char[] splitter = new char[1] { ':' };
            string[] splitted = result.Split(splitter[0]);

            switch (splitted[0])
            {
                case "Right":
                    LightHouseAngle = Convert.ToDouble(splitted[1]) - 90;
                    break;
                case "Left":
                    LightHouseAngle = 270 - Convert.ToDouble(splitted[1]);
                    break;
                case "Forward":
                    LightHouseAngle = 90;
                    break;
                default:
                    break;
            }
            resultArray[1] = (short)Math.Round(LightHouseAngle);
            if (manual)
            {
                manual = false;
                resultArray = manula_data;
            }
            //m_Active_Sound_Beacon_say(resultArray);
            return resultArray;
        }

        public short[] m_Active_Sound_Beacon(bool open)
        {
            if (!open)
                return new short[2] { -2, -2 };
            short[] resultArray = new short[2];
            resultArray[0] = 1;
            resultArray[1] = 90;
            if (manual)
            {
                manual = false;
                resultArray = manula_data;
            }
            //m_Active_Sound_Beacon_say(resultArray);
            return resultArray;
        }

        private void m_Active_Sound_Beacon_say(short[] array)
        {
            Vector3 objectPos;
            AudioEmitter emitter = new AudioEmitter();
            AudioListener listener = new AudioListener();
            float distance = array[0];
            double LightHouseAngle = array[1];
            if ((LightHouseAngle >= 22.5) && (LightHouseAngle <= 67.5))
            {
                objectPos = new Vector3((float)Math.Cos(LightHouseAngle * Math.PI / 180) * distance, 0, (float)Math.Sin(LightHouseAngle * Math.PI / 180) * distance);
                emitter.Position = objectPos;
                forwardRight.Apply3D(listener, emitter);
                forwardRight.Play();
            }
            else if (LightHouseAngle > 67.5 && LightHouseAngle <= 112.5)
            {
                objectPos = new Vector3((float)Math.Cos(LightHouseAngle * Math.PI / 180) * distance, 0, (float)Math.Sin(LightHouseAngle * Math.PI / 180) * distance);
                emitter.Position = objectPos;
                forward.Apply3D(listener, emitter);
                forward.Play();
            }
            else if (LightHouseAngle > 112.5 && LightHouseAngle <= 157.5)
            {
                objectPos = new Vector3((float)Math.Cos(LightHouseAngle * Math.PI / 180) * distance, 0, (float)Math.Sin(LightHouseAngle * Math.PI / 180) * distance);
                emitter.Position = objectPos;
                forwardLeft.Apply3D(listener, emitter);
                forwardLeft.Play();
            }

            else if (LightHouseAngle > 157.5 && LightHouseAngle <= 202.5)
            {
                objectPos = new Vector3((float)Math.Cos(LightHouseAngle * Math.PI / 180) * distance, 0, (float)Math.Sin(LightHouseAngle * Math.PI / 180) * distance);
                emitter.Position = objectPos;
                left.Apply3D(listener, emitter);
                left.Play();
            }
            else if (LightHouseAngle > 202.5 && LightHouseAngle <= 247.5)
            {
                objectPos = new Vector3((float)Math.Cos(LightHouseAngle * Math.PI / 180) * distance, 0, (float)Math.Sin(LightHouseAngle * Math.PI / 180) * distance);
                emitter.Position = objectPos;
                backwardLeft.Apply3D(listener, emitter);
                backwardLeft.Play();
            }
            else if ((LightHouseAngle > 247.5 && LightHouseAngle <= 270) || (LightHouseAngle >= -90 && LightHouseAngle < -67.5))// must be =
            {
                objectPos = new Vector3((float)Math.Cos(LightHouseAngle * Math.PI / 180) * distance, 0, (float)Math.Sin(LightHouseAngle * Math.PI / 180) * distance);
                emitter.Position = objectPos;
                backward.Apply3D(listener, emitter);
                backward.Play();
            }
            else if (LightHouseAngle >= -67.5 && LightHouseAngle < -22.5)
            {
                objectPos = new Vector3((float)Math.Cos(LightHouseAngle * Math.PI / 180) * distance, 0, (float)Math.Sin(LightHouseAngle * Math.PI / 180) * distance);
                emitter.Position = objectPos;
                backwardRight.Apply3D(listener, emitter);
                backwardRight.Play();
            }
            else if (LightHouseAngle >= -22.5 && LightHouseAngle < 22.5)
            {
                objectPos = new Vector3((float)Math.Cos(LightHouseAngle * Math.PI / 180) * distance, 0, (float)Math.Sin(LightHouseAngle * Math.PI / 180) * distance);
                emitter.Position = objectPos;
                right.Apply3D(listener, emitter);
                right.Play();
            }
        }

        enum c_Direction { Forward, Right, Left };
        private static string m_Get_Direction_and_Degree(System.Drawing.Point p1, System.Drawing.Point p2, System.Drawing.Point p3)
        {
            {
                //take care with this
                int x1, x2, x3, y1, y2, y3;
                double d1, d2, d3, Cos_alpha;
                //P1
                x1 = p1.X;
                y1 = p1.Y;
                //P2
                x2 = p2.X;
                y2 = p2.Y;
                //P3
                x3 = p3.X;
                y3 = p3.Y;

                //Check that p1 & p2 & p3 are not equal

                d1 = Math.Sqrt(Math.Pow(y2 - y1, 2) + Math.Pow(x2 - x1, 2));
                d2 = Math.Sqrt(Math.Pow(y3 - y2, 2) + Math.Pow(x3 - x2, 2));
                d3 = Math.Sqrt(Math.Pow(y3 - y1, 2) + Math.Pow(x3 - x1, 2));
                //Shall Formula
                Cos_alpha = (-Math.Pow(d3, 2) + Math.Pow(d1, 2) + Math.Pow(d2, 2)) /
                    (2 * d1 * d2);
                //Cos_alpha in [-1,1]  Cos(180)= -1 Cos(90)= 0   Cos(0) = 1
                if (Cos_alpha > 1)
                    Cos_alpha = 1;
                else if (Cos_alpha < -1)
                    Cos_alpha = -1;
                double alpha_radian = Math.Acos(Cos_alpha);
                double alpha_degree = Math.Round((alpha_radian * 180) / Math.PI);//convert to degree
                if (Cos_alpha == -1)//[-1 , - 0.7]
                {
                    return (c_Direction.Forward.ToString() + ":" + alpha_degree.ToString());
                }
                else //either Left or Right
                {
                    //translate to (0,0)

                    int X1, X2, X3, Y1, Y2, Y3;
                    int mx = 0, my = 0;
                    X1 = mx;
                    Y1 = my;

                    my += x2 - x1;
                    mx += y2 - y1;

                    X2 = mx;
                    Y2 = my;

                    my += x3 - x2;
                    mx += y3 - y2;

                    X3 = mx;
                    Y3 = my;

                    x1 = X1; x2 = X2; x3 = X3;
                    y1 = Y1; y2 = Y2; y3 = Y3;


                    // where is the point 2? it is in part (1 | 2 | 3 | 4)
                    if (x2 >= 0)
                    {
                        // in part 1 | 4
                        if (x2 == 0)
                        {
                            if (x3 > 0)
                            {
                                if (y2 > 0)
                                    return (c_Direction.Right.ToString() + ":" + alpha_degree.ToString());
                                else
                                    return (c_Direction.Left.ToString() + ":" + alpha_degree.ToString());
                            }
                            else
                            {
                                if (y2 > 0)
                                    return (c_Direction.Left.ToString() + ":" + alpha_degree.ToString());
                                else
                                    return (c_Direction.Right.ToString() + ":" + alpha_degree.ToString());
                            }
                        }
                        else
                        {
                            float slope = (float)y2 / (float)x2;
                            //in part 1 | 4
                            if (slope * x3 > y3)
                                return (c_Direction.Right.ToString() + ":" + alpha_degree.ToString());
                            else
                                return (c_Direction.Left.ToString() + ":" + alpha_degree.ToString());
                        }
                    }
                    else
                    {
                        // in part 2 | 3
                        float slope = (float)y2 / (float)x2;
                        if (slope * x3 > y3)
                            return (c_Direction.Left.ToString() + ":" + alpha_degree.ToString());
                        else
                            return (c_Direction.Right.ToString() + ":" + alpha_degree.ToString());
                    }
                }
            }
        }
    }
}