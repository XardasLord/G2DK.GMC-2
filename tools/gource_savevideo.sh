ffmpeg -y -r 60 -f image2pipe -vcodec ppm -i output.ppm -vcodec wmv1 -r 60 -qscale 0 out.wmv