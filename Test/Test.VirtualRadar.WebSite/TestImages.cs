﻿// Copyright © 2010 onwards, Andrew Whewell
// All rights reserved.
//
// Redistribution and use of this software in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
//    * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
//    * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
//    * Neither the name of the author nor the names of the program's contributors may be used to endorse or promote products derived from this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE AUTHORS OF THE SOFTWARE BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

// ===========================================================================================
//                           This file is generated by a T4 script
//   ANY CHANGES MADE DIRECTLY TO THIS FILE WILL BE LOST THE NEXT TIME THE FILE IS GENERATED
// ===========================================================================================

using System.Drawing;

namespace Test.VirtualRadar.WebSite
{
    /// <summary>
    /// A static class that exposes all of the images in the common resources.
    /// </summary>
    /// <remarks><para>
    /// These images are read/write properties. The application never writes to the properties but
    /// a plugin that wanted to change the graphics used by the application could do so by assigning
    /// new images to the appropriate properties of the class. Care should be taken to allow enough
    /// room on images that are rotated by the website and to replace existing images with images
    /// of the same dimensions and colour depth.
    /// </para><para>
    /// Assigning null to an image resets it back to the default image.
    /// </para></remarks>
    public static class TestImages
    {
        private static readonly byte[] _AltitudeImageTest_01_png_Stock = BinaryResources.Copy("TestImages.AltitudeImageTest-01.png");
        private static byte[] _AltitudeImageTest_01_png;
        /// <summary>
        /// Gets or sets the AltitudeImageTest_01_png image bytes.
        /// </summary>
        /// <remarks>
        /// <img src="../TestImages/AltitudeImageTest-01.png" alt="" title="AltitudeImageTest_01_png" />
        /// </remarks>
        public static byte[] AltitudeImageTest_01_png
        {
            get { return _AltitudeImageTest_01_png ?? _AltitudeImageTest_01_png_Stock; }
            set { _AltitudeImageTest_01_png = value; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="AltitudeImageTest_01_png"/> image has been customised or not.
        /// </summary>
        public static bool AltitudeImageTest_01_png_IsCustom
        {
            get { return _AltitudeImageTest_01_png != null; }
        }

        private static Bitmap _AltitudeImageTest_01_png_Bitmap;
        /// <summary>
        /// Gets the image as a System.Drawing.Bitmap.
        public static Bitmap AltitudeImageTest_01_png_Bitmap
        {
            get {
                if(_AltitudeImageTest_01_png_Bitmap == null) {
                    using(var stream = new System.IO.MemoryStream(_AltitudeImageTest_01_png_Stock)) {
                        _AltitudeImageTest_01_png_Bitmap = (Bitmap)Bitmap.FromStream(stream);
                    }
                }
                return _AltitudeImageTest_01_png_Bitmap;
            }
        }

        private static global::VirtualRadar.Interface.Drawing.IImage _AltitudeImageTest_01_png_IImage;
        /// <summary>
        /// Gets the image as a System.Drawing.Bitmap.
        public static global::VirtualRadar.Interface.Drawing.IImage AltitudeImageTest_01_png_IImage
        {
            get {
                if(_AltitudeImageTest_01_png_IImage == null) {
                    _AltitudeImageTest_01_png_IImage = global::InterfaceFactory.Factory.ResolveSingleton<global::VirtualRadar.Interface.Drawing.IImageFile>()
                        .LoadFromByteArray(_AltitudeImageTest_01_png_Stock);
                }
                return _AltitudeImageTest_01_png_IImage;
            }
        }

        private static readonly byte[] _DLH_bmp_Stock = BinaryResources.Copy("TestImages.DLH.bmp");
        private static byte[] _DLH_bmp;
        /// <summary>
        /// Gets or sets the DLH_bmp image bytes.
        /// </summary>
        /// <remarks>
        /// <img src="../TestImages/DLH.bmp" alt="" title="DLH_bmp" />
        /// </remarks>
        public static byte[] DLH_bmp
        {
            get { return _DLH_bmp ?? _DLH_bmp_Stock; }
            set { _DLH_bmp = value; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="DLH_bmp"/> image has been customised or not.
        /// </summary>
        public static bool DLH_bmp_IsCustom
        {
            get { return _DLH_bmp != null; }
        }

        private static Bitmap _DLH_bmp_Bitmap;
        /// <summary>
        /// Gets the image as a System.Drawing.Bitmap.
        public static Bitmap DLH_bmp_Bitmap
        {
            get {
                if(_DLH_bmp_Bitmap == null) {
                    using(var stream = new System.IO.MemoryStream(_DLH_bmp_Stock)) {
                        _DLH_bmp_Bitmap = (Bitmap)Bitmap.FromStream(stream);
                    }
                }
                return _DLH_bmp_Bitmap;
            }
        }

        private static global::VirtualRadar.Interface.Drawing.IImage _DLH_bmp_IImage;
        /// <summary>
        /// Gets the image as a System.Drawing.Bitmap.
        public static global::VirtualRadar.Interface.Drawing.IImage DLH_bmp_IImage
        {
            get {
                if(_DLH_bmp_IImage == null) {
                    _DLH_bmp_IImage = global::InterfaceFactory.Factory.ResolveSingleton<global::VirtualRadar.Interface.Drawing.IImageFile>()
                        .LoadFromByteArray(_DLH_bmp_Stock);
                }
                return _DLH_bmp_IImage;
            }
        }

        private static readonly byte[] _OversizedLogo_bmp_Stock = BinaryResources.Copy("TestImages.OversizedLogo.bmp");
        private static byte[] _OversizedLogo_bmp;
        /// <summary>
        /// Gets or sets the OversizedLogo_bmp image bytes.
        /// </summary>
        /// <remarks>
        /// <img src="../TestImages/OversizedLogo.bmp" alt="" title="OversizedLogo_bmp" />
        /// </remarks>
        public static byte[] OversizedLogo_bmp
        {
            get { return _OversizedLogo_bmp ?? _OversizedLogo_bmp_Stock; }
            set { _OversizedLogo_bmp = value; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="OversizedLogo_bmp"/> image has been customised or not.
        /// </summary>
        public static bool OversizedLogo_bmp_IsCustom
        {
            get { return _OversizedLogo_bmp != null; }
        }

        private static Bitmap _OversizedLogo_bmp_Bitmap;
        /// <summary>
        /// Gets the image as a System.Drawing.Bitmap.
        public static Bitmap OversizedLogo_bmp_Bitmap
        {
            get {
                if(_OversizedLogo_bmp_Bitmap == null) {
                    using(var stream = new System.IO.MemoryStream(_OversizedLogo_bmp_Stock)) {
                        _OversizedLogo_bmp_Bitmap = (Bitmap)Bitmap.FromStream(stream);
                    }
                }
                return _OversizedLogo_bmp_Bitmap;
            }
        }

        private static global::VirtualRadar.Interface.Drawing.IImage _OversizedLogo_bmp_IImage;
        /// <summary>
        /// Gets the image as a System.Drawing.Bitmap.
        public static global::VirtualRadar.Interface.Drawing.IImage OversizedLogo_bmp_IImage
        {
            get {
                if(_OversizedLogo_bmp_IImage == null) {
                    _OversizedLogo_bmp_IImage = global::InterfaceFactory.Factory.ResolveSingleton<global::VirtualRadar.Interface.Drawing.IImageFile>()
                        .LoadFromByteArray(_OversizedLogo_bmp_Stock);
                }
                return _OversizedLogo_bmp_IImage;
            }
        }

        private static readonly byte[] _Picture_120x140_Resized_60x40_png_Stock = BinaryResources.Copy("TestImages.Picture-120x140-Resized-60x40.png");
        private static byte[] _Picture_120x140_Resized_60x40_png;
        /// <summary>
        /// Gets or sets the Picture_120x140_Resized_60x40_png image bytes.
        /// </summary>
        /// <remarks>
        /// <img src="../TestImages/Picture-120x140-Resized-60x40.png" alt="" title="Picture_120x140_Resized_60x40_png" />
        /// </remarks>
        public static byte[] Picture_120x140_Resized_60x40_png
        {
            get { return _Picture_120x140_Resized_60x40_png ?? _Picture_120x140_Resized_60x40_png_Stock; }
            set { _Picture_120x140_Resized_60x40_png = value; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="Picture_120x140_Resized_60x40_png"/> image has been customised or not.
        /// </summary>
        public static bool Picture_120x140_Resized_60x40_png_IsCustom
        {
            get { return _Picture_120x140_Resized_60x40_png != null; }
        }

        private static Bitmap _Picture_120x140_Resized_60x40_png_Bitmap;
        /// <summary>
        /// Gets the image as a System.Drawing.Bitmap.
        public static Bitmap Picture_120x140_Resized_60x40_png_Bitmap
        {
            get {
                if(_Picture_120x140_Resized_60x40_png_Bitmap == null) {
                    using(var stream = new System.IO.MemoryStream(_Picture_120x140_Resized_60x40_png_Stock)) {
                        _Picture_120x140_Resized_60x40_png_Bitmap = (Bitmap)Bitmap.FromStream(stream);
                    }
                }
                return _Picture_120x140_Resized_60x40_png_Bitmap;
            }
        }

        private static global::VirtualRadar.Interface.Drawing.IImage _Picture_120x140_Resized_60x40_png_IImage;
        /// <summary>
        /// Gets the image as a System.Drawing.Bitmap.
        public static global::VirtualRadar.Interface.Drawing.IImage Picture_120x140_Resized_60x40_png_IImage
        {
            get {
                if(_Picture_120x140_Resized_60x40_png_IImage == null) {
                    _Picture_120x140_Resized_60x40_png_IImage = global::InterfaceFactory.Factory.ResolveSingleton<global::VirtualRadar.Interface.Drawing.IImageFile>()
                        .LoadFromByteArray(_Picture_120x140_Resized_60x40_png_Stock);
                }
                return _Picture_120x140_Resized_60x40_png_IImage;
            }
        }

        private static readonly byte[] _Picture_120x140_png_Stock = BinaryResources.Copy("TestImages.Picture-120x140.png");
        private static byte[] _Picture_120x140_png;
        /// <summary>
        /// Gets or sets the Picture_120x140_png image bytes.
        /// </summary>
        /// <remarks>
        /// <img src="../TestImages/Picture-120x140.png" alt="" title="Picture_120x140_png" />
        /// </remarks>
        public static byte[] Picture_120x140_png
        {
            get { return _Picture_120x140_png ?? _Picture_120x140_png_Stock; }
            set { _Picture_120x140_png = value; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="Picture_120x140_png"/> image has been customised or not.
        /// </summary>
        public static bool Picture_120x140_png_IsCustom
        {
            get { return _Picture_120x140_png != null; }
        }

        private static Bitmap _Picture_120x140_png_Bitmap;
        /// <summary>
        /// Gets the image as a System.Drawing.Bitmap.
        public static Bitmap Picture_120x140_png_Bitmap
        {
            get {
                if(_Picture_120x140_png_Bitmap == null) {
                    using(var stream = new System.IO.MemoryStream(_Picture_120x140_png_Stock)) {
                        _Picture_120x140_png_Bitmap = (Bitmap)Bitmap.FromStream(stream);
                    }
                }
                return _Picture_120x140_png_Bitmap;
            }
        }

        private static global::VirtualRadar.Interface.Drawing.IImage _Picture_120x140_png_IImage;
        /// <summary>
        /// Gets the image as a System.Drawing.Bitmap.
        public static global::VirtualRadar.Interface.Drawing.IImage Picture_120x140_png_IImage
        {
            get {
                if(_Picture_120x140_png_IImage == null) {
                    _Picture_120x140_png_IImage = global::InterfaceFactory.Factory.ResolveSingleton<global::VirtualRadar.Interface.Drawing.IImageFile>()
                        .LoadFromByteArray(_Picture_120x140_png_Stock);
                }
                return _Picture_120x140_png_IImage;
            }
        }

        private static readonly byte[] _Picture_120x80_Resized_60x40_png_Stock = BinaryResources.Copy("TestImages.Picture-120x80-Resized-60x40.png");
        private static byte[] _Picture_120x80_Resized_60x40_png;
        /// <summary>
        /// Gets or sets the Picture_120x80_Resized_60x40_png image bytes.
        /// </summary>
        /// <remarks>
        /// <img src="../TestImages/Picture-120x80-Resized-60x40.png" alt="" title="Picture_120x80_Resized_60x40_png" />
        /// </remarks>
        public static byte[] Picture_120x80_Resized_60x40_png
        {
            get { return _Picture_120x80_Resized_60x40_png ?? _Picture_120x80_Resized_60x40_png_Stock; }
            set { _Picture_120x80_Resized_60x40_png = value; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="Picture_120x80_Resized_60x40_png"/> image has been customised or not.
        /// </summary>
        public static bool Picture_120x80_Resized_60x40_png_IsCustom
        {
            get { return _Picture_120x80_Resized_60x40_png != null; }
        }

        private static Bitmap _Picture_120x80_Resized_60x40_png_Bitmap;
        /// <summary>
        /// Gets the image as a System.Drawing.Bitmap.
        public static Bitmap Picture_120x80_Resized_60x40_png_Bitmap
        {
            get {
                if(_Picture_120x80_Resized_60x40_png_Bitmap == null) {
                    using(var stream = new System.IO.MemoryStream(_Picture_120x80_Resized_60x40_png_Stock)) {
                        _Picture_120x80_Resized_60x40_png_Bitmap = (Bitmap)Bitmap.FromStream(stream);
                    }
                }
                return _Picture_120x80_Resized_60x40_png_Bitmap;
            }
        }

        private static global::VirtualRadar.Interface.Drawing.IImage _Picture_120x80_Resized_60x40_png_IImage;
        /// <summary>
        /// Gets the image as a System.Drawing.Bitmap.
        public static global::VirtualRadar.Interface.Drawing.IImage Picture_120x80_Resized_60x40_png_IImage
        {
            get {
                if(_Picture_120x80_Resized_60x40_png_IImage == null) {
                    _Picture_120x80_Resized_60x40_png_IImage = global::InterfaceFactory.Factory.ResolveSingleton<global::VirtualRadar.Interface.Drawing.IImageFile>()
                        .LoadFromByteArray(_Picture_120x80_Resized_60x40_png_Stock);
                }
                return _Picture_120x80_Resized_60x40_png_IImage;
            }
        }

        private static readonly byte[] _Picture_120x80_png_Stock = BinaryResources.Copy("TestImages.Picture-120x80.png");
        private static byte[] _Picture_120x80_png;
        /// <summary>
        /// Gets or sets the Picture_120x80_png image bytes.
        /// </summary>
        /// <remarks>
        /// <img src="../TestImages/Picture-120x80.png" alt="" title="Picture_120x80_png" />
        /// </remarks>
        public static byte[] Picture_120x80_png
        {
            get { return _Picture_120x80_png ?? _Picture_120x80_png_Stock; }
            set { _Picture_120x80_png = value; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="Picture_120x80_png"/> image has been customised or not.
        /// </summary>
        public static bool Picture_120x80_png_IsCustom
        {
            get { return _Picture_120x80_png != null; }
        }

        private static Bitmap _Picture_120x80_png_Bitmap;
        /// <summary>
        /// Gets the image as a System.Drawing.Bitmap.
        public static Bitmap Picture_120x80_png_Bitmap
        {
            get {
                if(_Picture_120x80_png_Bitmap == null) {
                    using(var stream = new System.IO.MemoryStream(_Picture_120x80_png_Stock)) {
                        _Picture_120x80_png_Bitmap = (Bitmap)Bitmap.FromStream(stream);
                    }
                }
                return _Picture_120x80_png_Bitmap;
            }
        }

        private static global::VirtualRadar.Interface.Drawing.IImage _Picture_120x80_png_IImage;
        /// <summary>
        /// Gets the image as a System.Drawing.Bitmap.
        public static global::VirtualRadar.Interface.Drawing.IImage Picture_120x80_png_IImage
        {
            get {
                if(_Picture_120x80_png_IImage == null) {
                    _Picture_120x80_png_IImage = global::InterfaceFactory.Factory.ResolveSingleton<global::VirtualRadar.Interface.Drawing.IImageFile>()
                        .LoadFromByteArray(_Picture_120x80_png_Stock);
                }
                return _Picture_120x80_png_IImage;
            }
        }

        private static readonly byte[] _Picture_140x80_Resized_60x40_png_Stock = BinaryResources.Copy("TestImages.Picture-140x80-Resized-60x40.png");
        private static byte[] _Picture_140x80_Resized_60x40_png;
        /// <summary>
        /// Gets or sets the Picture_140x80_Resized_60x40_png image bytes.
        /// </summary>
        /// <remarks>
        /// <img src="../TestImages/Picture-140x80-Resized-60x40.png" alt="" title="Picture_140x80_Resized_60x40_png" />
        /// </remarks>
        public static byte[] Picture_140x80_Resized_60x40_png
        {
            get { return _Picture_140x80_Resized_60x40_png ?? _Picture_140x80_Resized_60x40_png_Stock; }
            set { _Picture_140x80_Resized_60x40_png = value; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="Picture_140x80_Resized_60x40_png"/> image has been customised or not.
        /// </summary>
        public static bool Picture_140x80_Resized_60x40_png_IsCustom
        {
            get { return _Picture_140x80_Resized_60x40_png != null; }
        }

        private static Bitmap _Picture_140x80_Resized_60x40_png_Bitmap;
        /// <summary>
        /// Gets the image as a System.Drawing.Bitmap.
        public static Bitmap Picture_140x80_Resized_60x40_png_Bitmap
        {
            get {
                if(_Picture_140x80_Resized_60x40_png_Bitmap == null) {
                    using(var stream = new System.IO.MemoryStream(_Picture_140x80_Resized_60x40_png_Stock)) {
                        _Picture_140x80_Resized_60x40_png_Bitmap = (Bitmap)Bitmap.FromStream(stream);
                    }
                }
                return _Picture_140x80_Resized_60x40_png_Bitmap;
            }
        }

        private static global::VirtualRadar.Interface.Drawing.IImage _Picture_140x80_Resized_60x40_png_IImage;
        /// <summary>
        /// Gets the image as a System.Drawing.Bitmap.
        public static global::VirtualRadar.Interface.Drawing.IImage Picture_140x80_Resized_60x40_png_IImage
        {
            get {
                if(_Picture_140x80_Resized_60x40_png_IImage == null) {
                    _Picture_140x80_Resized_60x40_png_IImage = global::InterfaceFactory.Factory.ResolveSingleton<global::VirtualRadar.Interface.Drawing.IImageFile>()
                        .LoadFromByteArray(_Picture_140x80_Resized_60x40_png_Stock);
                }
                return _Picture_140x80_Resized_60x40_png_IImage;
            }
        }

        private static readonly byte[] _Picture_140x80_png_Stock = BinaryResources.Copy("TestImages.Picture-140x80.png");
        private static byte[] _Picture_140x80_png;
        /// <summary>
        /// Gets or sets the Picture_140x80_png image bytes.
        /// </summary>
        /// <remarks>
        /// <img src="../TestImages/Picture-140x80.png" alt="" title="Picture_140x80_png" />
        /// </remarks>
        public static byte[] Picture_140x80_png
        {
            get { return _Picture_140x80_png ?? _Picture_140x80_png_Stock; }
            set { _Picture_140x80_png = value; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="Picture_140x80_png"/> image has been customised or not.
        /// </summary>
        public static bool Picture_140x80_png_IsCustom
        {
            get { return _Picture_140x80_png != null; }
        }

        private static Bitmap _Picture_140x80_png_Bitmap;
        /// <summary>
        /// Gets the image as a System.Drawing.Bitmap.
        public static Bitmap Picture_140x80_png_Bitmap
        {
            get {
                if(_Picture_140x80_png_Bitmap == null) {
                    using(var stream = new System.IO.MemoryStream(_Picture_140x80_png_Stock)) {
                        _Picture_140x80_png_Bitmap = (Bitmap)Bitmap.FromStream(stream);
                    }
                }
                return _Picture_140x80_png_Bitmap;
            }
        }

        private static global::VirtualRadar.Interface.Drawing.IImage _Picture_140x80_png_IImage;
        /// <summary>
        /// Gets the image as a System.Drawing.Bitmap.
        public static global::VirtualRadar.Interface.Drawing.IImage Picture_140x80_png_IImage
        {
            get {
                if(_Picture_140x80_png_IImage == null) {
                    _Picture_140x80_png_IImage = global::InterfaceFactory.Factory.ResolveSingleton<global::VirtualRadar.Interface.Drawing.IImageFile>()
                        .LoadFromByteArray(_Picture_140x80_png_Stock);
                }
                return _Picture_140x80_png_IImage;
            }
        }

        private static readonly byte[] _Picture_700x400_png_Stock = BinaryResources.Copy("TestImages.Picture-700x400.png");
        private static byte[] _Picture_700x400_png;
        /// <summary>
        /// Gets or sets the Picture_700x400_png image bytes.
        /// </summary>
        /// <remarks>
        /// <img src="../TestImages/Picture-700x400.png" alt="" title="Picture_700x400_png" />
        /// </remarks>
        public static byte[] Picture_700x400_png
        {
            get { return _Picture_700x400_png ?? _Picture_700x400_png_Stock; }
            set { _Picture_700x400_png = value; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="Picture_700x400_png"/> image has been customised or not.
        /// </summary>
        public static bool Picture_700x400_png_IsCustom
        {
            get { return _Picture_700x400_png != null; }
        }

        private static Bitmap _Picture_700x400_png_Bitmap;
        /// <summary>
        /// Gets the image as a System.Drawing.Bitmap.
        public static Bitmap Picture_700x400_png_Bitmap
        {
            get {
                if(_Picture_700x400_png_Bitmap == null) {
                    using(var stream = new System.IO.MemoryStream(_Picture_700x400_png_Stock)) {
                        _Picture_700x400_png_Bitmap = (Bitmap)Bitmap.FromStream(stream);
                    }
                }
                return _Picture_700x400_png_Bitmap;
            }
        }

        private static global::VirtualRadar.Interface.Drawing.IImage _Picture_700x400_png_IImage;
        /// <summary>
        /// Gets the image as a System.Drawing.Bitmap.
        public static global::VirtualRadar.Interface.Drawing.IImage Picture_700x400_png_IImage
        {
            get {
                if(_Picture_700x400_png_IImage == null) {
                    _Picture_700x400_png_IImage = global::InterfaceFactory.Factory.ResolveSingleton<global::VirtualRadar.Interface.Drawing.IImageFile>()
                        .LoadFromByteArray(_Picture_700x400_png_Stock);
                }
                return _Picture_700x400_png_IImage;
            }
        }

        private static readonly byte[] _TestSquare_bmp_Stock = BinaryResources.Copy("TestImages.TestSquare.bmp");
        private static byte[] _TestSquare_bmp;
        /// <summary>
        /// Gets or sets the TestSquare_bmp image bytes.
        /// </summary>
        /// <remarks>
        /// <img src="../TestImages/TestSquare.bmp" alt="" title="TestSquare_bmp" />
        /// </remarks>
        public static byte[] TestSquare_bmp
        {
            get { return _TestSquare_bmp ?? _TestSquare_bmp_Stock; }
            set { _TestSquare_bmp = value; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="TestSquare_bmp"/> image has been customised or not.
        /// </summary>
        public static bool TestSquare_bmp_IsCustom
        {
            get { return _TestSquare_bmp != null; }
        }

        private static Bitmap _TestSquare_bmp_Bitmap;
        /// <summary>
        /// Gets the image as a System.Drawing.Bitmap.
        public static Bitmap TestSquare_bmp_Bitmap
        {
            get {
                if(_TestSquare_bmp_Bitmap == null) {
                    using(var stream = new System.IO.MemoryStream(_TestSquare_bmp_Stock)) {
                        _TestSquare_bmp_Bitmap = (Bitmap)Bitmap.FromStream(stream);
                    }
                }
                return _TestSquare_bmp_Bitmap;
            }
        }

        private static global::VirtualRadar.Interface.Drawing.IImage _TestSquare_bmp_IImage;
        /// <summary>
        /// Gets the image as a System.Drawing.Bitmap.
        public static global::VirtualRadar.Interface.Drawing.IImage TestSquare_bmp_IImage
        {
            get {
                if(_TestSquare_bmp_IImage == null) {
                    _TestSquare_bmp_IImage = global::InterfaceFactory.Factory.ResolveSingleton<global::VirtualRadar.Interface.Drawing.IImageFile>()
                        .LoadFromByteArray(_TestSquare_bmp_Stock);
                }
                return _TestSquare_bmp_IImage;
            }
        }

        private static readonly byte[] _TestSquare_png_Stock = BinaryResources.Copy("TestImages.TestSquare.png");
        private static byte[] _TestSquare_png;
        /// <summary>
        /// Gets or sets the TestSquare_png image bytes.
        /// </summary>
        /// <remarks>
        /// <img src="../TestImages/TestSquare.png" alt="" title="TestSquare_png" />
        /// </remarks>
        public static byte[] TestSquare_png
        {
            get { return _TestSquare_png ?? _TestSquare_png_Stock; }
            set { _TestSquare_png = value; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="TestSquare_png"/> image has been customised or not.
        /// </summary>
        public static bool TestSquare_png_IsCustom
        {
            get { return _TestSquare_png != null; }
        }

        private static Bitmap _TestSquare_png_Bitmap;
        /// <summary>
        /// Gets the image as a System.Drawing.Bitmap.
        public static Bitmap TestSquare_png_Bitmap
        {
            get {
                if(_TestSquare_png_Bitmap == null) {
                    using(var stream = new System.IO.MemoryStream(_TestSquare_png_Stock)) {
                        _TestSquare_png_Bitmap = (Bitmap)Bitmap.FromStream(stream);
                    }
                }
                return _TestSquare_png_Bitmap;
            }
        }

        private static global::VirtualRadar.Interface.Drawing.IImage _TestSquare_png_IImage;
        /// <summary>
        /// Gets the image as a System.Drawing.Bitmap.
        public static global::VirtualRadar.Interface.Drawing.IImage TestSquare_png_IImage
        {
            get {
                if(_TestSquare_png_IImage == null) {
                    _TestSquare_png_IImage = global::InterfaceFactory.Factory.ResolveSingleton<global::VirtualRadar.Interface.Drawing.IImageFile>()
                        .LoadFromByteArray(_TestSquare_png_Stock);
                }
                return _TestSquare_png_IImage;
            }
        }
    }
}
