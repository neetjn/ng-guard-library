using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Guard
{
    public class Profile
    {

            public string user;
            private string[] pKeys = new string[2];
            public bool Identified, Banned, Public, TyE, PyE, DyE;

            public Profile(string user, bool refresh = false)
            {

                this.user = user;
                this.Identified = User.Guard.Access("public", user, true);
                this.Banned = User.Guard.Banned(user);
                this.Public = User.Guard.Access("public", user);
                this.TyE = User.Guard.Access("TyE", user);
                this.PyE = User.Guard.Access("PyE", user);
                this.DyE = User.Guard.Access("DyE", user);
            
                if (refresh)
                    Refresh();
            }

        //REFRESH_PROFILE_INFORMATION
            public void Update()
            {

                while (true)
                {

                    this.Identified = User.Guard.Access("public", user, true);
                    this.Banned = User.Guard.Banned(user);
                    this.Public = User.Guard.Access("public", user);
                    this.TyE = User.Guard.Access("TyE", user);
                    this.PyE = User.Guard.Access("PyE", user);
                    this.DyE = User.Guard.Access("DyE", user);
                    Thread.Sleep(6750);
                }
            }      
            public void Refresh()
            {

                Thread update = new Thread(Update);
                update.IsBackground = true;
                update.Start();
            }
    }
}
