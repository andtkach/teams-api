namespace Teams.Services.Core.Models.Common
{
    using System;
    using Newtonsoft.Json;

    public class ModelBase
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }
        
        public string BaseType => GetType().Name;
        
        public DateTime CreatedDate { get; set; }
        
        public DateTime ModifiedDate { get; set; }

        public Guid Author { get; set; }

        public double Version
        {
            get => (this._version == 0) ? 1.0 : this._version; set => this._version = value;
        }

        private double _version { get; set; }

        public void ApplyDefaults(bool isModified = false)
        {
            if (!isModified)
            {
                this.Id = Guid.NewGuid();
                this.CreatedDate = DateTime.UtcNow;
            }

            this.ModifiedDate = DateTime.UtcNow;
        }
    }
}
