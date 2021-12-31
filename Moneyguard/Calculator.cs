using System;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class frmCalculator : Form
    {
        static public bool active = false;
        bool isNewEntry = false, isInfinityException = false, isRepeatLastOperation = false;
        double dblResult = 0, dblOperand = 0;
        char chPreviousOperator = new char();
        public frmCalculator()
        {
            InitializeComponent();
            active = true;
            this.FormClosed += (o, e) => { active = false; };
            foreach (Control ctrl in Controls) ctrl.KeyDown += txtResult_KeyDown;
        }
        private void UpdateOperand(object sender, EventArgs e)
        {
            if (!isInfinityException)
            {
                if (isNewEntry)
                {
                    txtResult.Text = "0";
                    isNewEntry = false;
                }
                if (isRepeatLastOperation)
                {
                    chPreviousOperator = '\0';
                    dblResult = 0;
                }
                if (!(txtResult.Text == "0" && (Button)sender == btn0) && !(((Button)sender) == btnDecimalPoint && txtResult.Text.Contains(",")))
                    txtResult.Text = (txtResult.Text == "0" && ((Button)sender) == btnDecimalPoint) ? "0," : ((txtResult.Text == "0") ? ((Button)sender).Text : txtResult.Text + ((Button)sender).Text);
            }
        }
        private void ClearOperator(object sender, EventArgs e)
        {
            isInfinityException = false;
            txtResult.Text = "0";
        }
        private void ChangeSign(object sender, EventArgs e)
        {
            if (!isInfinityException)
            {
                dblResult = double.Parse(txtResult.Text) * -1;
                txtResult.Text = dblResult.ToString();
            }
        }

        private void OperatorFound(object sender, EventArgs e)
        {
            if (!isInfinityException)
            {
                if (chPreviousOperator == '\0')
                {
                    chPreviousOperator = ((Button)sender).Text[0];
                    dblResult = double.Parse(txtResult.Text);
                }
                else if (isNewEntry)
                    chPreviousOperator = ((Button)sender).Text[0];
                else
                {
                    Operate(dblResult, chPreviousOperator, double.Parse(txtResult.Text));
                    chPreviousOperator = ((Button)sender).Text[0];
                }
                isNewEntry = true;
                isRepeatLastOperation = false;
            }
        }

        private void txtResult_KeyDown(object sender, KeyEventArgs e)
        {
            if (!((Control)sender).Focused) return;
            if (e.KeyCode == Keys.Add) { OperatorFound(btnAdd, null); }
            if (e.KeyCode == Keys.Subtract) { OperatorFound(btnSubstract, null); }
            if (e.KeyCode == Keys.Multiply) { OperatorFound(btnMultiply, null); }
            if (e.KeyCode == Keys.Divide) { OperatorFound(btnDivide, null); }
            if (e.KeyCode == Keys.Cancel || e.KeyCode == Keys.Delete) { ClearAll(btnClearAll, null); }
            if (e.KeyCode == Keys.Back) { ClearOperator(btnClearCurrentOperand, null); }
            if (e.KeyCode == Keys.Return) { Equals(btnEquals, null); }
            if (e.KeyCode == Keys.Oemcomma || e.KeyCode == Keys.OemPeriod || e.KeyCode == Keys.Decimal) { UpdateOperand(btnDecimalPoint, null); }

            if (e.Modifiers == Keys.None)
            {
                if (e.KeyCode == Keys.NumPad0 || e.KeyCode == Keys.D0) { UpdateOperand(btn0, null); }
                if (e.KeyCode == Keys.NumPad1 || e.KeyCode == Keys.D1) { UpdateOperand(btn1, null); }
                if (e.KeyCode == Keys.NumPad2 || e.KeyCode == Keys.D2) { UpdateOperand(btn2, null); }
                if (e.KeyCode == Keys.NumPad3 || e.KeyCode == Keys.D3) { UpdateOperand(btn3, null); }
                if (e.KeyCode == Keys.NumPad4 || e.KeyCode == Keys.D4) { UpdateOperand(btn4, null); }
                if (e.KeyCode == Keys.NumPad5 || e.KeyCode == Keys.D5) { UpdateOperand(btn5, null); }
                if (e.KeyCode == Keys.NumPad6 || e.KeyCode == Keys.D6) { UpdateOperand(btn6, null); }
                if (e.KeyCode == Keys.NumPad7 || e.KeyCode == Keys.D7) { UpdateOperand(btn7, null); }
                if (e.KeyCode == Keys.NumPad8 || e.KeyCode == Keys.D8) { UpdateOperand(btn8, null); }
                if (e.KeyCode == Keys.NumPad9 || e.KeyCode == Keys.D9) { UpdateOperand(btn9, null); }
            }
            Console.WriteLine("Modifiers:_"+ e.Modifiers+ "_Key:_" + e.KeyCode);
        }

        void Operate(double dblPreviousResult, char chPreviousOperator, double dblOperand)
        {
            switch (chPreviousOperator)
            {
                case '+':
                    txtResult.Text = (dblResult = (dblPreviousResult + dblOperand)).ToString();
                    break;
                case '-':
                    txtResult.Text = (dblResult = (dblPreviousResult - dblOperand)).ToString();
                    break;
                case '*':
                    txtResult.Text = (dblResult = (dblPreviousResult * dblOperand)).ToString();
                    break;
                case '/':
                    if (dblOperand == 0)
                    {
                        txtResult.Text = "Cannot divide by zero";
                        isInfinityException = true;
                    }
                    else
                        txtResult.Text = (dblResult = (dblPreviousResult / dblOperand)).ToString();
                    break;
            }
        }
        private void Equals(object sender, EventArgs e)
        {
            if (!isInfinityException)
            {
                if (!isRepeatLastOperation)
                {
                    dblOperand = double.Parse(txtResult.Text);
                    isRepeatLastOperation = true;
                }
                Operate(dblResult, chPreviousOperator, dblOperand);
                isNewEntry = true;
            }
        }
        private void ClearAll(object sender, EventArgs e)
        {
            isInfinityException = isRepeatLastOperation = false;
            dblOperand = dblResult = 0; txtResult.Text = "0";
            isNewEntry = true;
            chPreviousOperator = '\0';
        }


    }
}
