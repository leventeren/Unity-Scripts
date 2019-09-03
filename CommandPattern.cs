using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace CommandPattern
{
    public class InputHandler : MonoBehaviour
    {
        //Tuşlarla kontrol ettiğimiz kutu
        public Transform boxTrans;
        //İhtiyacımız olan diger butonlar
        private Command buttonW, buttonS, buttonA, buttonD, buttonB, buttonZ, buttonR;
        //tekrar ve geri almak icin Command List oluşturuyoruz.
         public static List<Command> oldCommands = new List<Command>();
        //Kutunun başlangıç konumu
        private Vector3 boxStartPos;
        //Coroutine resetlemek icin
        private Coroutine replayCoroutine;
        //Tekrarlamak istersek
        public static bool shouldStartReplay;
        /Oynarken butona basmamak icin
        private bool isReplaying;
        void Start()
        {
            //commandlari bind ediyoruz
            buttonB = new DoNothing();
            buttonW = new MoveForward();
            buttonS = new MoveReverse();
            buttonA = new MoveLeft();
            buttonD = new MoveRight();
            buttonZ = new UndoCommand();
            buttonR = new ReplayCommand();
            boxStartPos = boxTrans.position;
        }
        void Update()
        {
            if (!isReplaying)
            {
                HandleInput();
            }
            StartReplay();
        }
        //Hangi butona basarsak o butonu bind etmek icin if ile kontrol ediyoruz. 
        public void HandleInput()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                buttonA.Execute(boxTrans, buttonA);
            }
            else if (Input.GetKeyDown(KeyCode.B))
            {
                buttonB.Execute(boxTrans, buttonB);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                buttonD.Execute(boxTrans, buttonD);
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                buttonR.Execute(boxTrans, buttonZ);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                buttonS.Execute(boxTrans, buttonS);
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                buttonW.Execute(boxTrans, buttonW);
            }
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                buttonZ.Execute(boxTrans, buttonZ);
            }
        }
        //Tekrar icin
        void StartReplay()
        {
            if (shouldStartReplay && oldCommands.Count > 0)
            {
                shouldStartReplay = false;
                //Stop the coroutine so it starts from the beginning
                if (replayCoroutine != null)
                {
                    StopCoroutine(replayCoroutine);
                }
                //Burda Tekrar basliyor
                replayCoroutine = StartCoroutine(ReplayCommands(boxTrans));
            }
        }
        //replay yani tekrar coroutini
        IEnumerator ReplayCommands(Transform boxTrans)
        {
            //hareket ettirmiyoruz.
            isReplaying = true;
            
            //start pozisyonundan hareket ettiriyoruz
            boxTrans.position = boxStartPos;
            for (int i = 0; i < oldCommands.Count; i++)
            {
                //Kutuyu geçerli command ile hareket ettirmek icin.
                oldCommands[i].Move(boxTrans);
                yield return new WaitForSeconds(0.3f);
            }
            //tekrar hareket icin
            isReplaying = false;
        }
    }
}


//namespace kullanarak

using UnityEngine;
using System.Collections;
using System.Collections.Generic;namespace CommandPattern
{
    //parent class
    public abstract class Command
    {
        //Butona bastığımızda kutu ne kadar uzaklıkta hareket ediyor.
        protected float moveDistance = 1f;        //Kayit etmek icin (command)
        public abstract void Execute(Transform boxTrans, Command command);//eski commandi geri almak icin
        public virtual void Undo(Transform boxTrans) { }     
        public virtual void Move(Transform boxTrans) { }
    }
    //
    // Child sinif
    //
    public class MoveForward : Command
    {
        //Butona bastigimizda cagrilir
        public override void Execute(Transform boxTrans, Command command)
        {
           
            Move(boxTrans);
            
            //Commandi kaydediyoruz
            InputHandler.oldCommands.Add(command);
        }
        //Eski commandi geri alıyoruz
        public override void Undo(Transform boxTrans)
        {
            boxTrans.Translate(-boxTrans.forward * moveDistance);
        }      
        public override void Move(Transform boxTrans)
        {
            boxTrans.Translate(boxTrans.forward * moveDistance);
        }
    }
    public class MoveReverse : Command
    {
        
        public override void Execute(Transform boxTrans, Command command)
        {        
            Move(boxTrans);        
            InputHandler.oldCommands.Add(command);
        }        
        public override void Undo(Transform boxTrans)
        {
            boxTrans.Translate(boxTrans.forward * moveDistance);
        }        
        public override void Move(Transform boxTrans)
        {
            boxTrans.Translate(-boxTrans.forward * moveDistance);
        }
    }
    public class MoveLeft : Command
    {
    public override void Execute(Transform boxTrans, Command command)
        {
            Move(boxTrans);
            InputHandler.oldCommands.Add(command);
        }       
        public override void Undo(Transform boxTrans)
        {
            boxTrans.Translate(boxTrans.right * moveDistance);
        }       
        public override void Move(Transform boxTrans)
        {
            boxTrans.Translate(-boxTrans.right * moveDistance);
        }
    }
    public class MoveRight : Command
    {        
        public override void Execute(Transform boxTrans, Command command)
        {        
            Move(boxTrans);        
            InputHandler.oldCommands.Add(command);
        }        
        public override void Undo(Transform boxTrans)
        {
            boxTrans.Translate(-boxTrans.right * moveDistance);
        }        
        public override void Move(Transform boxTrans)
        {
            boxTrans.Translate(boxTrans.right * moveDistance);
        }
    }
    public class DoNothing : Command
    {
        public override void Execute(Transform boxTrans, Command command){}
    }
    public class UndoCommand : Command
    {
        
        public override void Execute(Transform boxTrans, Command command)
        {
            List oldCommands = InputHandler.oldCommands;
            if (oldCommands.Count &amp;amp;amp;amp;amp;gt; 0)
            {
                Command latestCommand = oldCommands[oldCommands.Count - 1];//Command ile kutuyu hareket ettiriyoruz
                latestCommand.Undo(boxTrans);//Listeden commandi siliyoruz
                oldCommands.RemoveAt(oldCommands.Count - 1);
            }
        }
    }
    //Replay ediyoruz tum commandlari
    public class ReplayCommand : Command
    {
        public override void Execute(Transform boxTrans, Command command)
        {
            InputHandler.shouldStartReplay = true;
        }
    }
}
