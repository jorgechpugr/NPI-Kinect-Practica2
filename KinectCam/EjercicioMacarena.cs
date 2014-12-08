using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

namespace KinectCam
{
    class EjercicioMacarena
    {

        public struct ptMov
        {
            public float X;
            public float Y;
            public float Z;
        };

        public enum posturas
        {
            Nada,
            BrazoIzqArriba,
            BrazoDerArriba,
            ManoIzqCodoDer,
            ManoDerCodoIzq,
            BrazoIzqCabeza,
            BrazoDerCabeza,
            BrazoIzqCadDer,
            BrazoDerCadIzq
        };

        List<posturas> listaPosturas = new List<posturas>();
        List<List<ptMov>> listaMovimientos = new List<List<ptMov>>();
        List<ptMov> Inicial = new List<ptMov>();

        ptMov centerHip, hipRight, hipLeft, shoulderRight, shoulderLeft, shoulderCenter, elbowRight, elbowLeft,
            handRight, handLeft, head;



        public EjercicioMacarena()
        {
            this.centerHip = new ptMov();
            this.hipRight = new ptMov();
            this.hipLeft = new ptMov();
            this.shoulderRight = new ptMov();
            this.shoulderLeft = new ptMov();
            this.shoulderCenter = new ptMov();
            this.elbowRight = new ptMov();
            this.elbowLeft = new ptMov();
            this.handRight = new ptMov();
            this.handLeft = new ptMov();
            this.head = new ptMov();
            listaPosturas.Add(posturas.Nada);
        }

        private bool CompruebaPosIni(Skeleton skeleton)
        {
            damePuntos(skeleton);
            ptMov headAux = Inicial[0];
            ptMov shoulderCenterAux = Inicial[1];
            ptMov CenterHipAux = Inicial[2];

            if (Math.Abs(headAux.X - head.X) <= 0.05f && Math.Abs(headAux.Y - head.Y) <= 0.05f && Math.Abs(headAux.Z - head.Z) <= 0.05f
                && Math.Abs(shoulderCenterAux.X - shoulderCenter.X) <= 0.05f && Math.Abs(shoulderCenterAux.Y - shoulderCenter.Y) <= 0.05f && Math.Abs(shoulderCenterAux.Z - shoulderCenter.Z) <= 0.05f
                && Math.Abs(CenterHipAux.X - centerHip.X) <= 0.05f && Math.Abs(CenterHipAux.Y - centerHip.Y) <= 0.05f && Math.Abs(CenterHipAux.Z - centerHip.Z) <= 0.05f)
            {
                return true;
            }

            return false;
        }

        private bool GuardaPosturaInicial(Skeleton skeleton)
        {
            damePuntos(skeleton);
            if (Math.Abs(head.X - shoulderCenter.X) <= 0.05f && Math.Abs(shoulderCenter.X - centerHip.X) <= 0.05f
                && Math.Abs(handLeft.Z - handRight.Z) <= 0.05f && Math.Abs(handLeft.Y - handRight.Y) <= 0.05f)
            {
                Inicial.Add(head);
                Inicial.Add(shoulderCenter);
                Inicial.Add(centerHip);
                Inicial.Add(shoulderLeft);
                Inicial.Add(elbowLeft);
                Inicial.Add(handLeft);
                Inicial.Add(hipLeft);
                Inicial.Add(shoulderRight);
                Inicial.Add(elbowRight);
                Inicial.Add(handRight);
                Inicial.Add(hipRight);

                return true;
            }
            return false;       
        }

        public float getXinicial(int pos) { return Inicial[pos].X; }
        public float getYinicial(int pos) { return Inicial[pos].Y; }
        public float getZinicial(int pos) { return Inicial[pos].Z; }

        public int CompruebaMovimiento(Skeleton skeleton)
        {
            posturas postura = DimeUltimaPostura();

            if (postura == posturas.Nada)
            {
                if (GuardaPosturaInicial(skeleton))
                {
                    listaPosturas.Add(posturas.BrazoIzqArriba);
                    return 1; 
                }
            }
            else if (postura == posturas.BrazoIzqArriba)
            {
                LevantaBrazoDerecho(skeleton);
                return 2;                
            }
            else if (postura == posturas.BrazoDerArriba)
            {
                ManoIzquierdaCodoDerecho(skeleton);
                return 3;                
            }
            else if (postura == posturas.ManoIzqCodoDer)
            {
                ManoDerechaCodoIzquierdo(skeleton);
                return 4;               
            }
            else if (postura == posturas.ManoDerCodoIzq)
            {
                BrazoIzquierdoCabeza(skeleton);
                return 5;               
            }
            else if (postura == posturas.BrazoIzqCabeza)
            {
                BrazoDerechoCabeza(skeleton);
                return 6;               
            }
            else if (postura == posturas.BrazoDerCabeza)
            {
                BrazoIzquierdoCaderaDerecha(skeleton);
                return 7;                
            }
            else if (postura == posturas.BrazoIzqCadDer)
            {
                BrazoDerechoCaderaIzquierda(skeleton);
                return 8;
            }
            else if (postura == posturas.BrazoDerCadIzq)
            {
                return 9;
            }

            return -1;

        }

        private posturas DimeUltimaPostura()
        {
            return listaPosturas[listaPosturas.Count - 1];
        }

        public void Reset()
        {
            listaPosturas.Clear();
            listaPosturas.Add(posturas.Nada);
        }

        private void LevantaBrazoIzquierdo(Skeleton skeleton)
        {
            //Obtenemos la posicion de los puntos del esqueleto.
            damePuntos(skeleton);

            List<ptMov> mov = new List<ptMov>();

            //Comprobar si la lista esta vacia, entonces se comienza desde la posicion estatica
            if (listaMovimientos.Count == 0)
            {

                //Comprobar que se encuentra en la posicion inicial. (Cuerpo recto y manos pegadas al cuerpo)
                //if (CompruebaPosIni(skeleton))
                //{
                   // if (Math.Abs(handLeft.Z - handRight.Z) <= 0.05f && Math.Abs(handLeft.Y - handRight.Y) <= 0.05f)
                    //{
                        mov.Add(handLeft);
                        mov.Add(elbowLeft);
                        listaMovimientos.Add(mov);
                    //}
                //}

            }
            else
            {
                //Obtenemos el ultimo elemto de la lista
                /*mov = listaMovimientos[listaMovimientos.Count - 1];
                ptMov handLeftAux = mov[0];
                ptMov elbowLeftAux = mov[1];
                ptMov shoulderLeftIni= Inicial[3];
                */
                //Comprobamos si se ha levantado por completo
                //if (Math.Abs(shoulderLeftIni.Y - handLeft.Y) <= 0.05f)
                //if (Math.Abs(handLeft.Y - elbowLeft.Y) <= 0.05f && Math.Abs(elbowLeft.Y - shoulderLeft.Y) <= 0.05f)
                if (handLeft.Y > shoulderLeft.Y)
                {
                    //Cuando se levante por completo se añade a la lista de posturas y se vacia la lista de movimientos.
                    listaPosturas.Add(posturas.BrazoIzqArriba);
                    listaMovimientos.Clear();
                }
                //Comparamos el movimiento anterior con el actual y vemos si ha levantado el brazo izquierdo.
              /*  else if (Math.Abs(handLeft.Y - handLeftAux.Y - 0.05f) > 0 && Math.Abs(elbowLeft.Y - elbowLeftAux.Y - 0.05f) > 0)
                {
                    mov.Add(handLeft);
                    mov.Add(elbowLeft);
                    listaMovimientos.Add(mov);
                }*/
                //Si el movimiento no es correcto no se añade a la lista y puede volver a la posicion anterior
            }
        }

        private void LevantaBrazoDerecho(Skeleton skeleton)
        {
            //Obtenemos la posicion de los puntos del esqueleto.
            damePuntos(skeleton);

            List<ptMov> mov = new List<ptMov>();

            //Comprobar si la lista esta vacia, entonces se comienza desde la posicion inicial.
            if (listaMovimientos.Count == 0)
            {
                //Comprobar que se encuentra en la posicion inicial. (Cuerpo recto,brazo izquierdo levantado y mano derecha pegada al cuerpo)
                //if (CompruebaPosIni(skeleton))
                //{
                 //   if (Math.Abs(handLeft.Y - elbowLeft.Y) <= 0.05f && Math.Abs(elbowLeft.Y - shoulderLeft.Y) <= 0.05f
                   //     && Math.Abs(handRight.X - elbowRight.X) <= 0.05f && Math.Abs(elbowRight.X - shoulderRight.X) <= 0.05f)
                    //{
                        mov.Add(handRight);
                        mov.Add(elbowRight);
                        listaMovimientos.Add(mov);
                    //}
                //}

            }
            else
            {
                //Obtenemos el ultimo elemto de la lista
                mov = listaMovimientos[listaMovimientos.Count - 1];
                ptMov handRightAux = mov[0];
                ptMov elbowRightAux = mov[1];

                //Comprobamos si se ha levantado por completo
                //if (Math.Abs(handRight.Y - elbowRight.Y) <= 0.05f && Math.Abs(elbowRight.Y - shoulderRight.Y) <= 0.05f)
                if (handRight.Y > shoulderRight.Y && elbowRight.Y > shoulderLeft.Y)
                {
                    //Cuando se levante por completo se añade a la lista de posturas y se vacia la lista de movimientos.
                    listaPosturas.Add(posturas.BrazoDerArriba);
                    listaMovimientos.Clear();
                }
                //Comparamos el movimiento anterior con el actual y vemos si ha levantado el brazo izquierdo.
                else if (Math.Abs(handRight.Y - handRightAux.Y - 0.05f) > 0 && Math.Abs(elbowRight.Y - elbowRightAux.Y - 0.05f) > 0)
                {
                    mov.Add(handRight);
                    mov.Add(elbowRight);
                    listaMovimientos.Add(mov);
                }
                //Si el movimiento no es correcto no se añade a la lista y puede volver a la posicion anterior
            }
        }

        private void ManoIzquierdaCodoDerecho(Skeleton skeleton)
        {
            //Obtenemos la posicion de los puntos del esqueleto.
            damePuntos(skeleton);

            List<ptMov> mov = new List<ptMov>();

            //Comprobar si la lista esta vacia, entonces se comienza desde la posicion inicial.
            if (listaMovimientos.Count == 0)
            {

                //Comprobar que se encuentra en la posicion inicial. (Cuerpo recto,brazos levantados)
                //if (Math.Abs(head.X - shoulderCenter.X) <= 0.05f && Math.Abs(shoulderCenter.X - centerHip.X) <= 0.05f)
                //{
                  //  if (Math.Abs(handLeft.Y - elbowLeft.Y) <= 0.05f && Math.Abs(elbowLeft.Y - shoulderLeft.Y) <= 0.05f
                    //    && Math.Abs(handRight.Y - elbowRight.Y) <= 0.05f && Math.Abs(elbowRight.Y - shoulderRight.Y) <= 0.05f)
                    //{
                        mov.Add(elbowRight);
                        mov.Add(handLeft);
                        listaMovimientos.Add(mov);
                    //}

                //}
            }
            else
            {
                //Obtenemos el ultimo elemto de la lista
                mov = listaMovimientos[listaMovimientos.Count - 1];
                ptMov elbowRightAux = mov[0];
                ptMov handLeftAux = mov[1];

                //Comprobamos si se ha colocado la mano izquierda en el codo derecho
                if (Math.Abs(handLeft.Y - elbowRight.Y) <= 0.05f && Math.Abs(handLeft.X - elbowRight.X) <= 0.05f)
                {
                    //Cuando se levante por completo se añade a la lista de posturas y se vacia la lista de movimientos.
                    listaPosturas.Add(posturas.ManoIzqCodoDer);
                    listaMovimientos.Clear();
                }
                //Comparamos el movimiento anterior con el actual y vemos si ha movido la mano hacia el codo.
                else if (Math.Abs(handLeft.Y - handLeftAux.Y) <= 0.05f && Math.Abs(handLeft.X - handLeftAux.X - 0.05f) < 0)
                {
                    mov.Add(elbowRight);
                    mov.Add(handLeft);
                    listaMovimientos.Add(mov);
                }
                //Si el movimiento no es correcto no se añade a la lista y puede volver a la posicion anterior
            }
        }

        private void ManoDerechaCodoIzquierdo(Skeleton skeleton)
        {
            //Obtenemos la posicion de los puntos del esqueleto.
            damePuntos(skeleton);

            List<ptMov> mov = new List<ptMov>();

            //Comprobar si la lista esta vacia, entonces se comienza desde la posicion inicial.
            if (listaMovimientos.Count == 0)
            {

                //Comprobar que se encuentra en la posicion inicial. (Cuerpo recto,brazo derecho levantado y mano izquierda en codo derecho)
               // if (CompruebaPosIni(skeleton))
                //{
                  //  if (Math.Abs(handLeft.Y - elbowRight.Y) <= 0.05f && Math.Abs(handLeft.X - elbowRight.X) <= 0.05f
                    //    && Math.Abs(handRight.Y - elbowRight.Y) <= 0.05f && Math.Abs(elbowRight.Y - shoulderRight.Y) <= 0.05f)
                    //{
                        mov.Add(elbowLeft);
                        mov.Add(handRight);
                        listaMovimientos.Add(mov);
                    //}

                //}
            }
            else
            {
                //Obtenemos el ultimo elemto de la lista
                mov = listaMovimientos[listaMovimientos.Count - 1];
                ptMov elbowLeftAux = mov[0];
                ptMov handRightAux = mov[1];

                //Comprobamos si se ha colocado la mano izquierda en el codo derecho
                if (Math.Abs(handRight.Y - elbowLeft.Y) <= 0.05f && Math.Abs(handRight.X - elbowLeft.X) <= 0.05f)
                {
                    //Cuando se levante por completo se añade a la lista de posturas y se vacia la lista de movimientos.
                    listaPosturas.Add(posturas.ManoDerCodoIzq);
                    listaMovimientos.Clear();
                }
                //Comparamos el movimiento anterior con el actual y vemos si ha movido la mano hacia el codo.
                else if (Math.Abs(handRight.Y - handRightAux.Y) <= 0.05f && Math.Abs(handRight.X - handRightAux.X - 0.05f) < 0)
                {
                    mov.Add(elbowLeft);
                    mov.Add(handRight);
                    listaMovimientos.Add(mov);
                }
                //Si el movimiento no es correcto no se añade a la lista y puede volver a la posicion anterior
            }
        }

        private void BrazoIzquierdoCabeza(Skeleton skeleton)
        {
            //Obtenemos la posicion de los puntos del esqueleto.
            damePuntos(skeleton);

            List<ptMov> mov = new List<ptMov>();

            //Comprobar si la lista esta vacia, entonces se comienza desde la posicion inicial.
            if (listaMovimientos.Count == 0)
            {

                //Comprobar que se encuentra en la posicion inicial. (Cuerpo recto,mano izquierda-codo derecho y mano derecha-codo izquierdo)
                //if (CompruebaPosIni(skeleton))
                //{
                  //  if (Math.Abs(handLeft.Y - elbowRight.Y) <= 0.05f && Math.Abs(handLeft.X - elbowRight.X) <= 0.05f
                    //    && Math.Abs(handRight.Y - handLeft.Y) <= 0.05f && Math.Abs(handRight.Y - handLeft.Y) <= 0.05f)
                    //{
                        mov.Add(handLeft);
                        listaMovimientos.Add(mov);
                    //}

                //}
            }
            else
            {
                //Obtenemos el ultimo elemto de la lista
                mov = listaMovimientos[listaMovimientos.Count - 1];
                ptMov handLeftAux = mov[0];

                //Comprobamos si se ha colocado la mano izquierda en la cabeza.
                if (handLeft.Y < head.Y && handLeft.Z < head.Z)
                {
                    //Cuando se levante por completo se añade a la lista de posturas y se vacia la lista de movimientos.
                    listaPosturas.Add(posturas.BrazoIzqCabeza);
                    listaMovimientos.Clear();
                }
                //Comparamos el movimiento anterior con el actual y vemos si ha movido la mano hacia la cabeza.
                else if (Math.Abs(handLeft.Z - handLeftAux.Z - 0.05f) < 0)
                {
                    mov.Add(handLeft);
                    listaMovimientos.Add(mov);
                }
                //Si el movimiento no es correcto no se añade a la lista y puede volver a la posicion anterior
            }
        }

        private void BrazoDerechoCabeza(Skeleton skeleton)
        {
            //Obtenemos la posicion de los puntos del esqueleto.
            damePuntos(skeleton);

            List<ptMov> mov = new List<ptMov>();

            //Comprobar si la lista esta vacia, entonces se comienza desde la posicion inicial.
            if (listaMovimientos.Count == 0)
            {

                //Comprobar que se encuentra en la posicion inicial. (Cuerpo recto,mano izquierda en cabeza)
                //if (CompruebaPosIni(skeleton))
                //{
                  //  if (Math.Abs(handLeft.Y - head.Y) <= 0.05f && Math.Abs(handLeft.X - head.X) <= 0.05f)
                   // {
                        mov.Add(handRight);
                        listaMovimientos.Add(mov);
                    //}

                //}
            }
            else
            {
                //Obtenemos el ultimo elemto de la lista
                mov = listaMovimientos[listaMovimientos.Count - 1];
                ptMov handRightAux = mov[0];

                //Comprobamos si se ha colocado la mano derecha en la cabeza.
                if (handRight.Y < head.Y && handRight.Z < head.Z)
                {
                    //Cuando se levante por completo se añade a la lista de posturas y se vacia la lista de movimientos.
                    listaPosturas.Add(posturas.BrazoDerCabeza);
                    listaMovimientos.Clear();
                }
                //Comparamos el movimiento anterior con el actual y vemos si ha movido la mano hacia la cabeza.
                else if (Math.Abs(handRight.Z - handRightAux.Z - 0.05f) < 0)
                {
                    mov.Add(handRight);
                    listaMovimientos.Add(mov);
                }
                //Si el movimiento no es correcto no se añade a la lista y puede volver a la posicion anterior
            }
        }

        private void BrazoIzquierdoCaderaDerecha(Skeleton skeleton)
        {
            //Obtenemos la posicion de los puntos del esqueleto.
            damePuntos(skeleton);

            List<ptMov> mov = new List<ptMov>();

            //Comprobar si la lista esta vacia, entonces se comienza desde la posicion inicial.
            if (listaMovimientos.Count == 0)
            {

                //Comprobar que se encuentra en la posicion inicial. (Cuerpo recto,manos en la cabeza)
                //if (Math.Abs(head.X - shoulderCenter.X) <= 0.05f && Math.Abs(shoulderCenter.X - centerHip.X) <= 0.05f)
                //{
                  //  if ((Math.Abs(handLeft.Y - head.Y) <= 0.05f && Math.Abs(handLeft.X - head.X) <= 0.05f)
                    //    && (Math.Abs(handRight.Y - head.Y) <= 0.05f && Math.Abs(handRight.X - head.X) <= 0.05f))
                    //{
                        mov.Add(handLeft);
                        listaMovimientos.Add(mov);
                    //}

                //}
            }
            else
            {
                //Obtenemos el ultimo elemto de la lista
                mov = listaMovimientos[listaMovimientos.Count - 1];
                ptMov handLeftAux = mov[0];

                //Comprobamos si se ha colocado la mano izquierda en la cadera derecha.
                if (handLeft.Y < hipRight.Y)
                {
                    //Cuando se levante por completo se añade a la lista de posturas y se vacia la lista de movimientos.
                    listaPosturas.Add(posturas.BrazoIzqCadDer);
                    listaMovimientos.Clear();
                }
                //Comparamos el movimiento anterior con el actual y vemos si ha movido la mano hacia la cadera.
                else if (Math.Abs(handLeft.Y - handLeftAux.Y - 0.05f) < 0)
                {
                    mov.Add(handLeft);
                    listaMovimientos.Add(mov);
                }
                //Si el movimiento no es correcto no se añade a la lista y puede volver a la posicion anterior
            }
        }

        private void BrazoDerechoCaderaIzquierda(Skeleton skeleton)
        {
            //Obtenemos la posicion de los puntos del esqueleto.
            damePuntos(skeleton);

            List<ptMov> mov = new List<ptMov>();

            //Comprobar si la lista esta vacia, entonces se comienza desde la posicion inicial.
            if (listaMovimientos.Count == 0)
            {

                //Comprobar que se encuentra en la posicion inicial. 
                //(Cuerpo recto,mano izquierda en cadera derecha y mano izquierda en cabeza)
                //if (CompruebaPosIni(skeleton))
                //{
                  //  if ((Math.Abs(handLeft.Y - hipRight.Y) <= 0.05f && Math.Abs(handLeft.X - hipRight.X) <= 0.05f)
                       // && (Math.Abs(handRight.Y - head.Y) <= 0.05f && Math.Abs(handRight.X - head.X) <= 0.05f))
                  //  {
                        mov.Add(handRight);
                        listaMovimientos.Add(mov);
                   // }

                //}
            }
            else
            {
                //Obtenemos el ultimo elemto de la lista
                mov = listaMovimientos[listaMovimientos.Count - 1];
                ptMov handRightAux = mov[0];

                //Comprobamos si se ha colocado la mano derecha en la cadera izquierda.
                if (handRight.Y < hipLeft.Y)
                {
                    //Cuando se levante por completo se añade a la lista de posturas y se vacia la lista de movimientos.
                    listaPosturas.Add(posturas.BrazoDerCadIzq);
                    listaMovimientos.Clear();
                }
                //Comparamos el movimiento anterior con el actual y vemos si ha movido la mano hacia la cadera.
                else if (Math.Abs(handRight.Y - handRightAux.Y - 0.05f) < 0)
                {
                    mov.Add(handRight);
                    listaMovimientos.Add(mov);
                }
                //Si el movimiento no es correcto no se añade a la lista y puede volver a la posicion anterior
            }
        }

        //Obtengo las coordenadas de los puntos.
        private void damePuntos(Skeleton skel)
        {
            if (skel.Joints[JointType.HipCenter].TrackingState == JointTrackingState.Tracked)
            {
                this.centerHip = new ptMov();
                //Obtengos puntos de la cadera. Punto central
                centerHip.X = skel.Joints[JointType.HipCenter].Position.X;
                centerHip.Y = skel.Joints[JointType.HipCenter].Position.Y;
                centerHip.Z = skel.Joints[JointType.HipCenter].Position.Z;
            }
            if (skel.Joints[JointType.HipRight].TrackingState == JointTrackingState.Tracked)
            {
                this.hipRight = new ptMov();
                //Obtengos puntos de la cadera derecha.
                hipRight.X = skel.Joints[JointType.HipRight].Position.X;
                hipRight.Y = skel.Joints[JointType.HipRight].Position.Y;
                hipRight.Z = skel.Joints[JointType.HipRight].Position.Z;
            }
            if (skel.Joints[JointType.HipLeft].TrackingState == JointTrackingState.Tracked)
            {
                this.hipLeft = new ptMov();
                //Obtengos puntos de la cadera izquierda.
                hipLeft.X = skel.Joints[JointType.HipLeft].Position.X;
                hipLeft.Y = skel.Joints[JointType.HipLeft].Position.Y;
                hipLeft.Z = skel.Joints[JointType.HipLeft].Position.Z;
            }
            if (skel.Joints[JointType.ShoulderRight].TrackingState == JointTrackingState.Tracked)
            {
                this.shoulderRight = new ptMov();
                //Puntos de hombro derecho.
                shoulderRight.X = skel.Joints[JointType.ShoulderRight].Position.X;
                shoulderRight.Y = skel.Joints[JointType.ShoulderRight].Position.Y;
                shoulderRight.Z = skel.Joints[JointType.ShoulderRight].Position.Z;
            }
            if (skel.Joints[JointType.ShoulderLeft].TrackingState == JointTrackingState.Tracked)
            {
                this.shoulderLeft = new ptMov();
                //Puntos de hombro izquierdo.
                shoulderLeft.X = skel.Joints[JointType.ShoulderLeft].Position.X;
                shoulderLeft.Y = skel.Joints[JointType.ShoulderLeft].Position.Y;
                shoulderLeft.Z = skel.Joints[JointType.ShoulderLeft].Position.Z;
            }
            if (skel.Joints[JointType.ShoulderCenter].TrackingState == JointTrackingState.Tracked)
            {
                this.shoulderCenter = new ptMov();
                //Puntos del centro de los hombros.
                shoulderCenter.X = skel.Joints[JointType.ShoulderCenter].Position.X;
                shoulderCenter.Y = skel.Joints[JointType.ShoulderCenter].Position.Y;
                shoulderCenter.Z = skel.Joints[JointType.ShoulderCenter].Position.Z;
            }
            if (skel.Joints[JointType.ElbowRight].TrackingState == JointTrackingState.Tracked)
            {
                this.elbowRight = new ptMov();
                //Puntos del codo derecho.
                elbowRight.X = skel.Joints[JointType.ElbowRight].Position.X;
                elbowRight.Y = skel.Joints[JointType.ElbowRight].Position.Y;
                elbowRight.Z = skel.Joints[JointType.ElbowRight].Position.Z;
            }
            if (skel.Joints[JointType.ElbowLeft].TrackingState == JointTrackingState.Tracked)
            {
                this.elbowLeft = new ptMov();
                //Puntos del codo izquierdo.
                elbowLeft.X = skel.Joints[JointType.ElbowLeft].Position.X;
                elbowLeft.Y = skel.Joints[JointType.ElbowLeft].Position.Y;
                elbowLeft.Z = skel.Joints[JointType.ElbowLeft].Position.Z;
            }
            if (skel.Joints[JointType.HandRight].TrackingState == JointTrackingState.Tracked)
            {
                this.handRight = new ptMov();
                //Puntos de la muñeca derecha.
                handRight.X = skel.Joints[JointType.HandRight].Position.X;
                handRight.Y = skel.Joints[JointType.HandRight].Position.Y;
                handRight.Z = skel.Joints[JointType.HandRight].Position.Z;
            }
            if (skel.Joints[JointType.HandLeft].TrackingState == JointTrackingState.Tracked)
            {
                this.handLeft = new ptMov();
                //Puntos de la muñeca irquierda.
                handLeft.X = skel.Joints[JointType.HandLeft].Position.X;
                handLeft.Y = skel.Joints[JointType.HandLeft].Position.Y;
                handLeft.Z = skel.Joints[JointType.HandLeft].Position.Z;
            }
            if (skel.Joints[JointType.Head].TrackingState == JointTrackingState.Tracked)
            {
                this.head = new ptMov();
                //Puntos de la cabeza.
                head.X = skel.Joints[JointType.Head].Position.X;
                head.Y = skel.Joints[JointType.Head].Position.Y;
                head.Z = skel.Joints[JointType.Head].Position.Z;
            }

        }
    }
}
