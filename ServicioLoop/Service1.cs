using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Timers;
using System.Configuration;

namespace ServicioLoop
{
    public partial class Service1 : ServiceBase
    {
        public Timer tiempo;
        public string fecha;
        public string filename;


        public Service1()
        {
            InitializeComponent();
            tiempo = new Timer();
            //sera igual a 10seg. tiempo para revisar archivos
            //Cada 6 Horas
            //tiempo.Interval = 21600000;
            
            tiempo.Elapsed += new ElapsedEventHandler(tiempo_Elapsed);
            fecha = DateTime.Now.ToShortDateString().Replace("/", "-");
            var tiempoInterval = ConfigurationManager.AppSettings["tiempoInterval"];
            tiempo.Interval = Convert.ToDouble(tiempoInterval);
            
        }

        //inicializar el servicio
        protected override void OnStart(string[] args)
        {
            tiempo.Enabled = true;
        }

        //finalizar el servicio
        protected override void OnStop()
        {
        }

        public void tiempo_Elapsed(object sender, EventArgs e)
        {
            var carpetaMover = ConfigurationManager.AppSettings["CarpetaMover"];
            var carpetaDirMover = ConfigurationManager.AppSettings["CarpetaDirectorio"];

            if (!Directory.Exists(carpetaMover + "Descargas " + fecha))
            {
                Directory.CreateDirectory(carpetaMover + "Descargas " + fecha);
            }

            foreach (string files in Directory.GetFiles(carpetaDirMover, "*.*"))
            {
                filename = Path.GetFileName(files);
                File.Move(files, carpetaMover + "Descargas " + fecha + "\\" + filename);

            }                 

        }                         

    }
}
