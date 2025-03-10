/*
 * Author: Alessandro de Oliveira Binhara
 * Igloobe Company
 * 
 * This file implements the CalibrationData class that stores and manages calibration 
 * information for the Igloobe system. It handles saving and loading calibration 
 * data for coordinate transformation between input devices and display coordinates.
 */
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Br.Com.IGloobe.Connector {

    [Serializable]
    public class CalibrationData {

        public int Width;
        public int Height;

        public float[] DstX;
        public float[] DstY;

        public float[] SrcX;
        public float[] SrcY;

        public CalibrationData(int width, int height, 
                                         float[] dstX, float[] dstY, 
                                         float[] srcX, float[] srcY){
            Width = width;
            Height = height;

            DstX = dstX;
            DstY = dstY;

            SrcX = srcX;
            SrcY = srcY;

            Store();
        }

        private void Store() {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Create(FileName());
            formatter.Serialize(file, this);
            file.Close();
        }

        private static string FileName() {
            return @"igloobe_" + Hardware.CurrentIGlooble + @".dat";
        }

        public static CalibrationData Restore(){
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(FileName(), FileMode.Open);
            CalibrationData result = (CalibrationData)formatter.Deserialize(file);
            file.Close();
            return result;
        }

        public static bool HasStoredData(){
            return File.Exists(FileName());
        }
    }
}
