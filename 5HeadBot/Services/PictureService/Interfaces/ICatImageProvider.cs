using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace _5HeadBot.Services.PictureService.Interfaces
{
    public interface ICatImageProvider
    {
        public Task<Stream> GetCatPictureAsync();
    }
}
