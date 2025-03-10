/*
 * Author: Alessandro de Oliveira Binhara
 * Igloobe Company
 * 
 * This file implements the Warper class that handles coordinate transformation 
 * and perspective correction for the Igloobe system. It performs the mathematical 
 * operations needed to map input coordinates to screen coordinates using matrix transformations.
 */
namespace Br.Com.IGloobe.Connector{

    public class Warper {

        private readonly float[] _srcX = new float[4];
        private readonly float[] _srcY = new float[4];
        private readonly float[] _dstX = new float[4];
        private readonly float[] _dstY = new float[4];
        private readonly float[] _srcMat = new float[16];
        private readonly float[] _dstMat = new float[16];
        private readonly float[] _warpMat = new float[16];
        private bool _dirty;

        public Warper() {
            Identity();
        }

        public void Identity() {
            Source(0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f, 1.0f, 1.0f);
            Destination(0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f, 1.0f, 1.0f);
            ComputeWarp();
        }

        public void Source(float x0, float y0, float x1, float y1, float x2, float y2, float x3, float y3){
	        _srcX[0] = x0;
	        _srcY[0] = y0;
	        _srcX[1] = x1;
	        _srcY[1] = y1;
	        _srcX[2] = x2;
	        _srcY[2] = y2;
	        _srcX[3] = x3;
	        _srcY[3] = y3;
            _dirty = true;
        }

        public void Destination(float x0, float y0, float x1, float y1, float x2, float y2, float x3, float y3){
	        _dstX[0] = x0;
	        _dstY[0] = y0;
	        _dstX[1] = x1;
	        _dstY[1] = y1;
	        _dstX[2] = x2;
	        _dstY[2] = y2;
	        _dstX[3] = x3;
	        _dstY[3] = y3;
            _dirty = true;
        }


        public void ComputeWarp() {
	        ComputeQuadToSquare(	_srcX[0],_srcY[0],
							        _srcX[1],_srcY[1],
							        _srcX[2],_srcY[2],
							        _srcX[3],_srcY[3],
							        _srcMat);

	        ComputeSquareToQuad(	_dstX[0], _dstY[0],
							        _dstX[1], _dstY[1],
							        _dstX[2], _dstY[2],
							        _dstX[3], _dstY[3],
							        _dstMat);

	        MultMats(_srcMat, _dstMat, _warpMat);
	        _dirty = false;
        }

        public void MultMats(float[] srcMat, float[] dstMat, float[] resMat) {
	        // DSTDO/CBB: could be faster, but not called often enough to matter
	        for (int r = 0; r < 4; r++) {
	            int ri = r * 4;
	            for (int c = 0; c < 4; c++) {
		            resMat[ri + c] = (srcMat[ri    ] * dstMat[c     ] +
				          srcMat[ri + 1] * dstMat[c +  4] +
				          srcMat[ri + 2] * dstMat[c +  8] +
				          srcMat[ri + 3] * dstMat[c + 12]);
		        }
	        }
        }

        public void ComputeSquareToQuad(float x0, float y0, float x1,  float y1,  float x2, float y2, float x3, float y3, float[] mat) {

	        float dx1 = x1 - x2, 	dy1 = y1 - y2;
	        float dx2 = x3 - x2, 	dy2 = y3 - y2;
	        float sx = x0 - x1 + x2 - x3;
	        float sy = y0 - y1 + y2 - y3;
	        float g = (sx * dy2 - dx2 * sy) / (dx1 * dy2 - dx2 * dy1);
	        float h = (dx1 * sy - sx * dy1) / (dx1 * dy2 - dx2 * dy1);
	        float a = x1 - x0 + g * x1;
	        float b = x3 - x0 + h * x3;
	        float c = x0;
	        float d = y1 - y0 + g * y1;
	        float e = y3 - y0 + h * y3;
	        float f = y0;

	        mat[ 0] = a;	mat[ 1] = d;	mat[ 2] = 0;	mat[ 3] = g;
	        mat[ 4] = b;	mat[ 5] = e;	mat[ 6] = 0;	mat[ 7] = h;
	        mat[ 8] = 0;	mat[ 9] = 0;	mat[10] = 1;	mat[11] = 0;
	        mat[12] = c;	mat[13] = f;	mat[14] = 0;	mat[15] = 1;
       }

        public void ComputeQuadToSquare(float x0, float y0, float x1, float y1, float x2, float y2, float x3, float y3, float[] mat) {
	        
            ComputeSquareToQuad(x0,y0,x1,y1,x2,y2,x3,y3, mat);

	        float a = mat[ 0],	d = mat[ 1],	g = mat[ 3];
	        float b = mat[ 4],	e = mat[ 5],	h = mat[ 7];
	        float c = mat[12],	f = mat[13];

	        float sa =     e - f * h;
	        float sb = c * h - b;
	        float sc = b * f - c * e;
	        float sd = f * g - d;
	        float se =     a - c * g;
	        float sf = c * d - a * f;
	        float sg = d * h - e * g;
	        float sh = b * g - a * h;
	        float si = a * e - b * d;

	        float idet = 1.0f / (a * sa + b * sd  + c * sg);

	        mat[ 0] = sa * idet;
            mat[ 1] = sd * idet;	
            mat[ 2] = 0;	
            mat[ 3] = sg * idet;
	        mat[ 4] = sb * idet;	
            mat[ 5] = se * idet;	
            mat[ 6] = 0;	
            mat[ 7] = sh * idet;
	        mat[ 8] = 0;	
            mat[ 9] = 0;
            mat[10] = 1;
            mat[11] = 0 ;
	        mat[12] = sc * idet;
            mat[13] = sf * idet;
            mat[14] = 0;
            mat[15] = si * idet;
        }

        public float[] WarpMatrix(){
	        return _warpMat;
        }

        public void Warp(float srcX, float srcY, ref float dstX, ref float dstY) {
            if (_dirty) ComputeWarp();
            Warp(_warpMat, srcX, srcY, ref dstX, ref dstY);
        }

        public static void Warp(float[] mat, float srcX, float srcY, ref float dstX, ref float dstY){
            float[] result = new float[4];
            result[0] = srcX * mat[0] + srcY*mat[4] + 1*mat[12];
            result[1] = srcX * mat[1] + srcY*mat[5]  + 1*mat[13];
            result[2] = srcX * mat[2] + srcY*mat[6] + 1*mat[14];
            result[3] = srcX * mat[3] + srcY*mat[7] + 1*mat[15];        
            dstX = result[0]/result[3];
		    dstY = result[1]/result[3];
        }
    }
}
