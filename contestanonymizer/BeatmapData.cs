﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace contestanonymizer
{
    public class BeatmapData : ObservableCollection<Beatmap>
    {
        public BeatmapData() //literally just an observable collection for beatmaps, for data binding purposes
        {

        }
    }
}
