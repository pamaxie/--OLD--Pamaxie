﻿namespace Pamaxie.MediaDetection.FileSpecifications
{
    public class JpegSpec : FileSpecification
    {
        public JpegSpec() 
            : base(3, new byte[] {0xFF, 0xD8}){}
    }
}