using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace StreamLibrary.src;

public class JpgCompression
{
	private EncoderParameter parameter;

	private ImageCodecInfo encoderInfo;

	private EncoderParameters encoderParams;

	public JpgCompression(int Quality)
	{
		parameter = new EncoderParameter(Encoder.Quality, Quality);
		encoderInfo = GetEncoderInfo("image/jpeg");
		encoderParams = new EncoderParameters(2);
		encoderParams.Param[0] = parameter;
		encoderParams.Param[1] = new EncoderParameter(Encoder.Compression, 2L);
	}

	public byte[] Compress(Bitmap bmp)
	{
		using MemoryStream memoryStream = new MemoryStream();
		bmp.Save(memoryStream, encoderInfo, encoderParams);
		return memoryStream.ToArray();
	}

	public void Compress(Bitmap bmp, ref Stream TargetStream)
	{
		bmp.Save(TargetStream, encoderInfo, encoderParams);
	}

	private ImageCodecInfo GetEncoderInfo(string mimeType)
	{
		ImageCodecInfo[] imageEncoders = ImageCodecInfo.GetImageEncoders();
		int num = imageEncoders.Length - 1;
		for (int i = 0; i <= num; i++)
		{
			if (imageEncoders[i].MimeType == mimeType)
			{
				return imageEncoders[i];
			}
		}
		return null;
	}
}
