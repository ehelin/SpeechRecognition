using System;
using NAudio.Wave;
using Shared.Dto;
using System.Collections.Generic;
using Windows.Storage;
using System.Threading.Tasks;

namespace BLL
{
    public class Perceptron
    {
        private StorageFile audioFile = null;

        public Perceptron(StorageFile audioFile)
        {
            this.audioFile = audioFile;
        }

        public bool IsListenCommand()
        {
            bool isCommand = false;

            if (audioFile != null)
            {
                //train perceptron here and return whether 'Fred' is matched or not
            }

            return isCommand;
        }
    }
}
