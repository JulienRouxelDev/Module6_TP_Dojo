﻿using BO;
using Dojo.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dojo.Models
{
    public class SamouraiVM
    {
		private Samourai samourai;
		private List<Arme> armesDisponibles;
		private List<ArtMartial> artMartiauxDisponibles;
		private int? idArme;
		private List<int> idsArtMartiaux;

		public List<ArtMartial> ArtMatiauxDisponible
		{
			get { return artMartiauxDisponibles; }
			set { artMartiauxDisponibles = value; }
		}

		public List<int> IdsArtMartiaux
		{
			get { return idsArtMartiaux; }
			set { idsArtMartiaux = value; }
		}

		public int? IdArme
		{
			get { return idArme; }
			set { idArme = value; }
		}


		public List<Arme> ArmesDisponibles
		{
			get { return this.armesDisponibles; }
			set { this.armesDisponibles = value; }
		}


		public Samourai Samourai
		{
			get { return this.samourai; }
			set { this.samourai = value; }
		}

	}
}