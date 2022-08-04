using MaterialSkin;
using MaterialSkin.Controls;
using MyWenxinTool;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChengTuoChecker
{
    public partial class Form1 : MaterialForm
    {
        public static object ObjcetMult = new object();
        public List<Form_SubImg> listForm = new List<Form_SubImg>();
        public Form1()
        {
            InitializeComponent();
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);

            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(
                         Primary.Pink100,
                         Primary.Pink200,
                          Primary.Pink100,
                         Accent.Blue400,//0xff9393
                         TextShade.WHITE);
            DrawerUseColors = true;
            DrawerHighlightWithAccent = true;
            DrawerBackgroundWithAccent = true;
            DrawerShowIconsWhenHidden = true;
            DrawerAutoShow = true;

            for (int i = 0; i < 15; i++)
            {
                listForm.Add(new Form_SubImg());
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            musicplay.PlayMusic(@"./music/hmbb.mp3");
            //this.Show(form_SubImg);   
            Task.Run(() => TaskVideo());

        }

        private void TaskVideo()
        {
            var capture = new VideoCapture(0);
            int sleepTime = (int)Math.Round(1000 / capture.Fps);
            
            using (Mat image = new Mat())
            {
                while (capture.Read(image))
                {
                    
                    //判断是否还有没有视频图像 
                    if (image.Empty())
                        break;
                    // 在picturebox中播放视频， 需要先转换成bitmap格式
                    Action action = new Action(() => {

                        

                        Task.Run(() => {
                            Mat outmat = new Mat();
                            TaskDect(image,out outmat);
                            var ms = outmat.ToMemoryStream();

                            lock(ObjcetMult)
                            {
                                pictureBox1.Image = (Bitmap)Image.FromStream(ms);

                            }
                            
                            ms.Dispose();
                        });
                       

                    });

                    base.BeginInvoke(action);
                    
                    Cv2.WaitKey(30);
                    GC.Collect();
                }

            }
               
        }

        private void TaskDect(Mat mat,out Mat outMat)
        {
            using (var face = new CascadeClassifier(@".\tree\haarcascade_frontalface_alt.xml"))
            {
                Mat Gray = new Mat();
                //把图片从彩色转灰度
                Cv2.CvtColor(mat, Gray, ColorConversionCodes.BGR2GRAY);

                Rect[] rects =  face.DetectMultiScale(Gray);

                if (rects.Length >= 0)
                {
                    for (int i = 0; i < rects.Length; i++)
                    {

                        //Cv2.Rectangle(mat, rects[i].TopLeft, rects[i].BottomRight, new OpenCvSharp.Scalar(0, 0, 255), 3);
                        int b = 0, g = 0, r = 255;

                        //绘制
                        int axis_x, axis_y;
                        for (int j = 0; j < rects[i].Height; j++)
                        { 
                            for (int n = 0; n < rects[i].Width; n++)
                            {
                                axis_y = rects[i].TopLeft.Y - mat.Height;
                                axis_x = rects[i].BottomRight.X - mat.Width;
 
                                //5*pow(x,2)-6*abs(x)*y+5*pow(y,2)==128
                                double left = 5 * Math.Pow(axis_x, 2) - 6 * Math.Abs(axis_x) * axis_y + 5 * Math.Pow(axis_y, 2);

                                mat.At<Vec3f>(j, n)[0] = b;
                                mat.At<Vec3f>(j, n)[1] = g;
                                mat.At<Vec3f>(j, n)[2] = r;
                                
                            }
                        }
                        outMat = mat;
                    }
                }

            }

            outMat = mat;

        }
        public static void ShowChildFormInMDI(Form childFrm, Form parentFrm)//显示子窗体


        {
            if (Application.OpenForms[childFrm.Name] != null)
            {
                Application.OpenForms[childFrm.Name].Activate();
                childFrm.Dispose();
            }
            else
            {
                childFrm.Show(parentFrm);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ShowChildFormInMDI(new Form_SubImg(), this);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            int baseLocalX = 0;
            int baseLocalY = 0;
            //Draw Heart 
            for (int i = 0; i < listForm.Count; i++)
            {
                if (i == 0)
                {
                    baseLocalX = listForm[i].Left;
                    baseLocalY = listForm[i].Top;
                }
                else
                {
                    listForm[i].Top = baseLocalY - 50;
                    listForm[i].Left = baseLocalX - 50;
                }


                listForm[i].Show(this);
            }
            
        }
    }
}
