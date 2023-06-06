
using System;
// it's required for reading/writing into the registry:
using Microsoft.Win32;
// and for the MessageBox function:
using System.Windows.Forms;

namespace NWP
{
    /// <summary>
    /// An useful class to read/write/delete/count registry keys
    /// </summary>
    public class ModifyRegistry
    {
        private bool showError = false;
        /// <summary>
        /// A property to show or hide error messages 
        /// (default = false)
        /// </summary>
        public bool ShowError
        {
            get { return showError; }
            set { showError = value; }
        }

        public string subKey = "SOFTWARE\\" + Application.ProductName.ToUpper();







        /// <summary>
        /// A property to set the SubKey value
        /// (default = "SOFTWARE\\" + Application.ProductName.ToUpper())
        /// </summary>
        public string SubKey
        {
            get { return subKey; }
            set { subKey = value; }
        }



        private RegistryKey baseRegistryKey = Registry.CurrentUser;

        /// <summary>
        /// A property to set the BaseRegistryKey value.
        /// (default = Registry.LocalMachine)
        /// </summary>
        public RegistryKey BaseRegistryKey
        {
            get { return baseRegistryKey; }
            set { baseRegistryKey = value; }
        }

        /* **************************************************************************
         * **************************************************************************/

        /// <summary>
        /// To read a registry key.
        /// input: KeyName (string)
        /// output: value (string) 
        /// </summary>
        public string Read(string KeyName)
        {
            // Opening the registry key
            RegistryKey rk = baseRegistryKey;
            // Open a subKey as read-only
            RegistryKey sk1 = rk.OpenSubKey(subKey);
            // If the RegistrySubKey doesn't exist -> (null)
            if (sk1 == null)
            {
                return null;
            }
            else
            {
                try
                {
                    // If the RegistryKey exists I get its value
                    // or null is returned.
                    return (string)sk1.GetValue(KeyName.ToUpper());
                }
                catch (Exception e)
                {
                    // AAAAAAAAAAARGH, an error!
                    ShowErrorMessage(e, "Reading registry " + KeyName.ToUpper());
                    return null;
                }
            }
        }
        //**************************************     create registory ************
        public string CreateRegistory(string Name)
        {
            try
            {

                Registry.CurrentUser.CreateSubKey("SOFTWARE\\" + Application.ProductName.ToUpper() + "\\" + Name);
            }
            catch (Exception ex)
            {

                ShowErrorMessage(ex, "Create registry " + Name.ToUpper());

            }

            return null;

        }
        //************** FRead File Name ***************

      
             
        public string[] FNAme()
        {
            RegistryKey rk = baseRegistryKey;
            RegistryKey sk1 = rk.OpenSubKey("SOFTWARE\\" + Application.ProductName.ToUpper()+"\\");
            String registryKey = Registry.CurrentUser + "SOFTWARE\\" + Application.ProductName.ToUpper();

            int i = 0;
            if (sk1 == null)
            {
                return null;
            }
            else
            {
                string[] names = new string[sk1.SubKeyCount];

                if (names.Length > 0)
                {
                    foreach (String subkeyName in sk1.GetSubKeyNames())
                    {
                        //names = new string[i + 1];

                        names[i] = subkeyName;
                        i++;
                    }
                }
                return names;
            }
            
            

            // Console.WriteLine(key.OpenSubKey(subkeyName).GetValue("DisplayName"));
            //    using (Microsoft.Win32.RegistryKey key = Registry.LocalMachine.OpenSubKey(registryKey))
            //    {
            //}


        }

        //*************************** fread file data 


        public string FRead(string KeyName, string Fname)
        {
            // Opening the registry key
            RegistryKey rk = baseRegistryKey;
            // Open a subKey as read-only
            RegistryKey sk1 = rk.OpenSubKey("SOFTWARE\\" + Application.ProductName.ToUpper() + "\\" + Fname);
            // If the RegistrySubKey doesn't exist -> (null)
            if (sk1 == null)
            {
                return null;
            }
            else
            {
                try
                {
                    // If the RegistryKey exists I get its value
                    // or null is returned.
                    return (string)sk1.GetValue(KeyName.ToUpper());
                }
                catch (Exception e)
                {
                    // AAAAAAAAAAARGH, an error!
                    ShowErrorMessage(e, "Reading registry " + KeyName.ToUpper());
                    return null;
                }
            }
        }




        //*************** write file data **************

        public bool FWrite(string KeyName, object Value, string Fname)
        {
            try
            {
                // Setting
                RegistryKey rk = baseRegistryKey;
                // I have to use CreateSubKey 
                // (create or open it if already exits), 
                // 'cause OpenSubKey open a subKey as read-only
                RegistryKey sk1 = rk.CreateSubKey("SOFTWARE\\" + Application.ProductName.ToUpper() + "\\" + Fname);
                // Save the value
                sk1.SetValue(KeyName.ToUpper(), Value);

                return true;
            }
            catch (Exception e)
            {
                // AAAAAAAAAAARGH, an error!
                ShowErrorMessage(e, "Writing registry " + KeyName.ToUpper());
                return false;
            }
        }

        //**************** File delete ****************

        public bool FDeleteKey(string Fname)
        {
            try
            {
                // Setting
                //RegistryKey rk = baseRegistryKey;
                //RegistryKey sk1 = rk.CreateSubKey("SOFTWARE\\" + Application.ProductName.ToUpper() + "\\" + Fname);
                //// If the RegistrySubKey doesn't exists -> (true)
                //if (sk1 == null)
                //    return true;
                //else
                //    sk1.DeleteSubKeyTree(Fname);
                Registry.CurrentUser.DeleteSubKey("SOFTWARE\\" + Application.ProductName.ToUpper() + "\\" + Fname);

                return true;
            }
            catch (Exception e)
            {
                // AAAAAAAAAAARGH, an error!
                ShowErrorMessage(e, "Deleting SubKey " + "SOFTWARE\\" + Application.ProductName.ToUpper() + "\\" + Fname);
                return false;
            }
        }

        //* ***************************** Rename File Name *********************************************

        public string ReNameRegistory(string OldName, string NewName)
        {
            try
            {

                RegistryKey rk = baseRegistryKey;
                RegistryKey sk1 = rk.OpenSubKey("SOFTWARE\\" + Application.ProductName.ToUpper());
                RegistryKey sk2 = rk.OpenSubKey("SOFTWARE\\" + Application.ProductName.ToUpper() + "\\" + OldName);
                String registryKey = Registry.CurrentUser + "SOFTWARE\\" + Application.ProductName.ToUpper();
                int i = 0;

                if (sk1 == null)
                {
                    return null;
                }
                else
                {
                    string[] names = new string[sk1.SubKeyCount];

                    foreach (String subkeyName in sk1.GetSubKeyNames())
                    {
                        if (subkeyName.Equals(OldName))
                        {
                            Registry.CurrentUser.CreateSubKey("SOFTWARE\\" + Application.ProductName.ToUpper() + "\\" + NewName);

                            foreach (string valuename in sk2.GetValueNames())
                            {
                                FWrite(valuename, sk2.GetValue(valuename), NewName);
                            }


                            Registry.CurrentUser.DeleteSubKey("SOFTWARE\\" + Application.ProductName.ToUpper() + "\\" + OldName);

                        }

                        names = new string[i + 1];

                        names[i] = subkeyName;
                        i++;
                    }
                }
            }
            catch (Exception ex)
            {

                ShowErrorMessage(ex, "Create registry " + NewName.ToUpper());

            }

            return null;

        }



        // **************************************************************************/

        /// <summary>
        /// To write into a registry key.
        /// input: KeyName (string) , Value (object)
        /// output: true or false 
        /// </summary>
        public bool Write(string KeyName, object Value)
        {
            try
            {
                // Setting
                RegistryKey rk = baseRegistryKey;
                // I have to use CreateSubKey 
                // (create or open it if already exits), 
                // 'cause OpenSubKey open a subKey as read-only
                RegistryKey sk1 = rk.CreateSubKey(subKey);
                // Save the value
                sk1.SetValue(KeyName.ToUpper(), Value);

                return true;
            }
            catch (Exception e)
            {
                // AAAAAAAAAAARGH, an error!
                ShowErrorMessage(e, "Writing registry " + KeyName.ToUpper());
                return false;
            }
        }

        /* **************************************************************************
         * **************************************************************************/

        /// <summary>
        /// To delete a registry key.
        /// input: KeyName (string)
        /// output: true or false 
        /// </summary>
        public bool DeleteKey(string KeyName)
        {
            try
            {
                // Setting
                RegistryKey rk = baseRegistryKey;
                RegistryKey sk1 = rk.CreateSubKey(subKey);
                // If the RegistrySubKey doesn't exists -> (true)
                if (sk1 == null)
                    return true;
                else
                    sk1.DeleteValue(KeyName);

                return true;
            }
            catch (Exception e)
            {
                // AAAAAAAAAAARGH, an error!
                ShowErrorMessage(e, "Deleting SubKey " + subKey);
                return false;
            }
        }

        /* **************************************************************************
         * **************************************************************************/

        /// <summary>
        /// To delete a sub key and any child.
        /// input: void
        /// output: true or false 
        /// </summary>
        public bool DeleteSubKeyTree()
        {
            try
            {
                // Setting
                RegistryKey rk = baseRegistryKey;
                RegistryKey sk1 = rk.OpenSubKey(subKey);
                // If the RegistryKey exists, I delete it
                if (sk1 != null)
                    rk.DeleteSubKeyTree(subKey);

                return true;
            }
            catch (Exception e)
            {
                // AAAAAAAAAAARGH, an error!
                ShowErrorMessage(e, "Deleting SubKey " + subKey);
                return false;
            }
        }

        /* **************************************************************************
         * **************************************************************************/

        /// <summary>
        /// Retrive the count of subkeys at the current key.
        /// input: void
        /// output: number of subkeys
        /// </summary>
        public int SubKeyCount()
        {
            try
            {
                // Setting
                RegistryKey rk = baseRegistryKey;
                RegistryKey sk1 = rk.OpenSubKey(subKey);
                // If the RegistryKey exists...
                if (sk1 != null)
                    return sk1.SubKeyCount;
                else
                    return 0;
            }
            catch (Exception e)
            {
                // AAAAAAAAAAARGH, an error!
                ShowErrorMessage(e, "Retriving subkeys of " + subKey);
                return 0;
            }
        }

        /* **************************************************************************
         * **************************************************************************/

        /// <summary>
        /// Retrive the count of values in the key.
        /// input: void
        /// output: number of keys
        /// </summary>
        public int ValueCount()
        {
            try
            {
                // Setting
                RegistryKey rk = baseRegistryKey;
                RegistryKey sk1 = rk.OpenSubKey(subKey);
                // If the RegistryKey exists...
                if (sk1 != null)
                    return sk1.ValueCount;
                else
                    return 0;
            }
            catch (Exception e)
            {
                // AAAAAAAAAAARGH, an error!
                ShowErrorMessage(e, "Retriving keys of " + subKey);
                return 0;
            }
        }

        /* **************************************************************************
         * **************************************************************************/

        private void ShowErrorMessage(Exception e, string Title)
        {
            if (showError == true)
                MessageBox.Show(e.Message,
                                Title
                                , MessageBoxButtons.OK
                                , MessageBoxIcon.Error);
        }
    }
}
