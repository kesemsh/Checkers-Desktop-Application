using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Ex05_DamkaGame
{
    public class ButtonDamka : Button
    {
        private Point m_LogicLocationOnBoard;
        public const int k_Width = 55;
        public const int k_Height = 55;

        public ButtonDamka(string m_ButtonDamakText, bool i_IsEnabled)
        {
            initiate(m_ButtonDamakText, i_IsEnabled);
        }

        private void initiate(string m_ButtonDamkaText, bool i_IsEnabled)
        {
            this.Text = m_ButtonDamkaText;
            this.Width = 55;
            this.Height = 55;
            this.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold);
            this.BackColor = Color.White;

            if (i_IsEnabled == false)
            {
                this.BackColor = Color.DarkGray;
                this.Enabled = false;
            }
        }

        public Point LogicLocationOnBoard
        {
            get 
            { 
                return m_LogicLocationOnBoard; 
            }

            set 
            { 
                m_LogicLocationOnBoard = value; 
            }
        }

        public void SetButtonDamkaText(string i_CellStr)
        {
            this.Text = i_CellStr;
        }
    }
}