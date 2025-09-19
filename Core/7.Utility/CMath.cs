using System;
using System.Drawing;
using Cognex.VisionPro;
using Insnex.Vision2D.Core;

namespace Core.Utility
{
    public class CMath
    {
        public double GetXFromLine(CogLine Line, double Yp)
        {
            return (Yp - Line.Y) / Math.Tan(Line.Rotation) + Line.X;
        }
        public double GetXFromLine(InsLine Line, double Yp)
        {
            return (Yp - Line.Y) / Math.Tan(Line.Rotation) + Line.X;
        }

        public double GetYFromLine(CogLine Line, double Xp)
        {
            return Math.Tan(Line.Rotation) * (Xp - Line.X) + Line.Y;
        }
        public double GetYFromLine(InsLine Line, double Xp)
        {
            return Math.Tan(Line.Rotation) * (Xp - Line.X) + Line.Y;
        }

        /// <summary>
        /// Get Y point on the line
        /// </summary>
        /// <param name="Line_X"></param>
        /// <param name="Line_Y"></param>
        /// <param name="Line_T_Rad">Radian</param>
        /// <param name="Yp"></param>
        /// <returns></returns>
        public double GetXFromLine(double Line_X, double Line_Y, double Line_T_Rad, double Yp)
        {
            return (Yp - Line_Y) / Math.Tan(Line_T_Rad) + Line_X;
        }
        /// <summary>
        /// Get X point on the line
        /// </summary>
        /// <param name="Line_X"></param>
        /// <param name="Line_Y"></param>
        /// <param name="Line_T_Rad"></param>
        /// <param name="Yp"></param>
        /// <returns></returns>
        public double GetYFromLine(double Line_X, double Line_Y, double Line_T_Rad, double Xp)
        {
            return Math.Tan(Line_T_Rad) * (Xp - Line_X) + Line_Y;
        }
        /// <summary>
        /// Get X point on the line
        /// </summary>
        /// <param name="X1">start point</param>
        /// <param name="Y1">start point</param>
        /// <param name="X2">end point</param>
        /// <param name="Y2">end point</param>
        /// <param name="Yp">input y value</param>
        /// <returns></returns>
        public double GetXFromLine(double X1, double Y1, double X2, double Y2, double Yp)
        {
            double Line_T = (Y2 - Y1) / (X2 - X1);
            return (Yp - Y1) / Line_T + X1;
        }
        /// <summary>
        /// Get Y point on the line
        /// </summary>
        /// <param name="X1">start point</param>
        /// <param name="Y1">start point</param>
        /// <param name="X2">end point</param>
        /// <param name="Y2">end point</param>
        /// <param name="Xp">input x value</param>
        /// <returns></returns>
        public double GetYFromLine(double X1, double Y1, double X2, double Y2, double Xp)
        {
            double Line_T = (Y2 - Y1) / (X2 - X1);
            return Line_T * (Xp - X1) + Y1;
        }

        public void GetIntersectLineLine(CogLine Line1, CogLine Line2, out double InterX, out double InterY)
        {
            double Line1_T = Math.Tan(Line1.Rotation);
            double Line2_T = Math.Tan(Line2.Rotation);
            InterX = (float)((Line1_T * Line1.X - Line1.Y - (Line2_T * Line2.X - Line2.Y)) / (Line1_T - Line2_T));
            InterY = GetYFromLine(Line1, InterX);
        }
        public void GetIntersectLineLine(CogLine Line1, CogLine Line2, out double InterX, out double InterY, out double InterT_Rad)
        {
            double Line1_T = Math.Tan(Line1.Rotation);
            double Line2_T = Math.Tan(Line2.Rotation);
            InterX = (float)((Line1_T * Line1.X - Line1.Y - (Line2_T * Line2.X - Line2.Y)) / (Line1_T - Line2_T));
            InterY = GetYFromLine(Line1, InterX);
            InterT_Rad = Line2.Rotation - Line1.Rotation;
        }
        public void GetIntersectLineLine(InsLine Line1, InsLine Line2, out double InterX, out double InterY, out double InterT_Rad)
        {
            double Line1_T = Math.Tan(Line1.Rotation);
            double Line2_T = Math.Tan(Line2.Rotation);
            InterX = (float)((Line1_T * Line1.X - Line1.Y - (Line2_T * Line2.X - Line2.Y)) / (Line1_T - Line2_T));
            InterY = GetYFromLine(Line1, InterX);
            InterT_Rad = Line2.Rotation - Line1.Rotation;
        }
        public void GetIntersectLineLine(double Line1_X, double Line1_Y, double Line1_T_Rad, double Line2_X, double Line2_Y, double Line2_T_Rad, out double InterX, out double InterY)
        {
            double Line1_T = Math.Tan(Line1_T_Rad);
            double Line2_T = Math.Tan(Line2_T_Rad);
            double b1 = Line1_Y - Line1_T * Line1_X;
            double b2 = Line2_Y - Line2_T * Line2_X;
            InterX = (b2 - b1) / (Line1_T - Line2_T);
            InterY = GetYFromLine(Line1_X, Line1_Y, Line1_T_Rad, InterX);
        }
        public void GetIntersectLineLine(double Line1_X, double Line1_Y, double Line1_T_Rad, double Line2_X, double Line2_Y, double Line2_T_Rad, out double InterX, out double InterY, out double InterT_Rad)
        {
            double Line1_T = Math.Tan(Line1_T_Rad);
            double Line2_T = Math.Tan(Line2_T_Rad);
            double b1 = Line1_Y - Line1_T * Line1_X;
            double b2 = Line2_Y - Line2_T * Line2_X;
            InterX = (b2 - b1) / (Line1_T - Line2_T);
            InterY = GetYFromLine(Line1_X, Line1_Y, Line1_T_Rad, InterX);
            InterT_Rad = Line2_T_Rad - Line1_T_Rad;
        }

        public double GetDistancePointToPoint(PointF Point1, PointF Point2)
        {
            return Math.Sqrt((Point1.X - Point2.X) * (Point1.X - Point2.X) + (Point1.Y - Point2.Y) * (Point1.Y - Point2.Y));
        }
        public double GetDistancePointToPoint(double X1, double Y1, double X2, double Y2)
        {
            return Math.Sqrt((X1 - X2) * (X1 - X2) + (Y1 - Y2) * (Y1 - Y2));
        }
        public void GetLine_XYT(double X1, double Y1, double X2, double Y2, out double Line_X, out double Line_Y, out double Line_T_Rad)
        {
            Line_X = X1;
            Line_Y = Y1;
            Line_T_Rad = Math.Atan((Y2 - Y1) / (X2 - X1));
        }

        public void GetLine_XYT(PointF point1, PointF point2, out double Line_X, out double Line_Y, out double Line_T_Rad)
        {
            Line_X = point1.X;
            Line_Y = point1.Y;
            Line_T_Rad = Math.Atan((point2.Y - point1.Y) / (point2.X - point1.X));
        }

        /// <summary>
        /// 25.01.26 NIS Check if the point position is betwwen two vertical lines
        /// </summary>
        /// <param name="Ver1_X"></param>
        /// <param name="Ver1_Y"></param>
        /// <param name="Ver1_T"></param>
        /// <param name="Ver2_X"></param>
        /// <param name="Ver2_Y"></param>
        /// <param name="Ver2_T"></param>
        /// <param name="Point"></param>
        /// <returns>true is in the area</returns>
        public bool IsBetweenTwoVerLines(double Ver1_X, double Ver1_Y, double Ver1_T, double Ver2_X, double Ver2_Y, double Ver2_T, PointF Point)
        {
            bool IsInTheArea = false;
            double X1 = GetXFromLine(Ver1_X, Ver1_Y, Ver1_T, Point.Y);
            double X2 = GetXFromLine(Ver2_X, Ver2_Y, Ver2_T, Point.Y);
            if(X1 < X2)
            {
                if (X1 <= Point.X && Point.X <= X2)
                    IsInTheArea = true;
            }
            else
            {
                if (X2 <= Point.X && Point.X <= X1)
                    IsInTheArea = true;
            }
            return IsInTheArea;
        }

        public bool IsBetweenTwoVerLines(PointF Ver1_Point, double Ver1_T, PointF Ver2_Point, double Ver2_T, PointF Point)
        {
            bool IsInTheArea = false;
            double X1 = GetXFromLine(Ver1_Point.X, Ver1_Point.Y, Ver1_T, Point.Y);
            double X2 = GetXFromLine(Ver2_Point.X, Ver2_Point.Y, Ver2_T, Point.Y);
            if (X1 < X2)
            {
                if (X1 <= Point.X && Point.X <= X2)
                    IsInTheArea = true;
            }
            else
            {
                if (X2 <= Point.X && Point.X <= X1)
                    IsInTheArea = true;
            }
            return IsInTheArea;
        }

        public bool IsBetweenTwoVerLines(InsLine VerLine1, InsLine VerLine2, double PosX, double PosY)
        {
            bool IsInTheArea = false;
            double X1 = GetXFromLine(VerLine1, PosY);
            double X2 = GetXFromLine(VerLine2, PosY);
            if (X1 < X2)
            {
                if (X1 <= PosX && PosX <= X2)
                    IsInTheArea = true;
            }
            else
            {
                if (X2 <= PosX && PosX <= X1)
                    IsInTheArea = true;
            }
            return IsInTheArea;
        }
        public bool IsBetweenTwoHorLines(InsLine HorLine1, InsLine HorLine2, double PosX, double PosY)
        {
            bool IsInTheArea = false;
            double Y1 = GetYFromLine(HorLine1, PosX);
            double Y2 = GetYFromLine(HorLine2, PosX);
            if (Y1 < Y2)
            {
                if (Y1 <= PosY && PosY <= Y2)
                    IsInTheArea = true;
            }
            else
            {
                if (Y2 <= PosY && PosY <= Y1)
                    IsInTheArea = true;
            }
            return IsInTheArea;
        }
    }
}
