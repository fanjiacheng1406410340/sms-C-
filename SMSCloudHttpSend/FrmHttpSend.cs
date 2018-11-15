using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace SMSCloudHttpSend
{
    public partial class FrmHttpSend : Form
    {
        protected String opTag = "SendSMS";
        public FrmHttpSend()
        {
            InitializeComponent();
        }

        /**
         * 
         * 普通发送短信
         * **/
        private void btnInvoke_Click(object sender, EventArgs e)
        {
            submitBtn.Enabled = false;
            exitBtn.Enabled = false;
            if (checkInput())
            {
                String _serverURL = txtServerURL.Text.Trim();
                String _data = null;
                String _account = txtUserName.Text.Trim();
                String _passWord = md5(txtPassWord.Text.Trim());
                 switch (opTag)
                {
                    case "SendSMS"://发送短信

                        SendSmsData sendSmsData = new SendSmsData();
                        sendSmsData.Phones=txtPhone.Text.Trim();
                        sendSmsData.Content= txtSmsContent.Text.Trim();
                        sendSmsData.Msgid= txtSmsId.Text.Trim();
                        sendSmsData.Sign= this.txtSign.Text.Trim();
                        sendSmsData.Subcode= this.txtSubCode.Text.Trim();
                        _data = this.packageSendSmsJsonData(_account, _passWord, sendSmsData);

                        break;
                    case "GetSMS"://获取上
                        _data = this.packageDeliverJsonData(_account, _passWord);
                        break;
                    case "GetReport"://获取状态报告
                        _data = this.packageReportJsonData(_account, _passWord);
                        break;

                    default:
                        break;
                }
                this.txtPostData.Text = _data;
                this.txtResponseData.Text=  postMethodConnServer(_serverURL, _data);
            }
            submitBtn.Enabled = true;
            exitBtn.Enabled = true;
        }

   
        /**
         * 
         * 普通发送短信，选择调用方法
         * 
         * **/
        private void rdo_Click(object sender, EventArgs e)
        {
            RadioButton _rdo = sender as RadioButton;
            opTag = _rdo.Tag.ToString();
            String _str = txtServerURL.Text.Trim().ToString();
            Boolean _isBlank = "".Equals(_str);

            int _idxHttp = -1;
            if (_str.Length > 7)
            {
                _idxHttp = _str.IndexOf("/http/", 7);
            }

            switch (opTag)
            {
                case "SendSMS"://发送短信
                    txtPhone.Enabled = true;
                    txtSmsId.Enabled = true;
                    txtSmsContent.Enabled = true;
                    txtSign.Enabled = true;
                    txtSubCode.Enabled = true;
                    if ("".Equals(txtSmsId.Text.Trim()))
                    {
                        generalNewData();
                        
                    }
                    if (!_isBlank && _idxHttp > -1)
                    {
                        txtServerURL.Text = _str.Substring(0, _str.IndexOf("/http/", 7)) + "/json/sms/Submit";
                    }
                    else
                    {
                        txtServerURL.Text = "http://www.dh3t.com/json/sms/Submit";
                    }
                    break;
                case "GetSMS"://获取上行
                    txtPhone.Enabled = false;
                    txtSmsId.Enabled = false;
                    txtSmsContent.Enabled = false;
                    txtSign.Enabled = false;
                    txtSubCode.Enabled = false;
                    if (!_isBlank && _idxHttp > -1)
                    {
                        txtServerURL.Text = _str.Substring(0, _str.IndexOf("/http/", 7)) + "/json/sms/Deliver";
                    }
                    else
                    {
                        txtServerURL.Text = "http://www.dh3t.com/json/sms/Deliver";
                    }
                    break;

                case "GetReport"://获取状态报告
                    txtPhone.Enabled = false;
                    txtSmsId.Enabled = false;
                    txtSmsContent.Enabled = false;
                    txtSign.Enabled = false;
                    txtSubCode.Enabled = false;

                    if (!_isBlank && _idxHttp > -1)
                    {
                        txtServerURL.Text = _str.Substring(0, _str.IndexOf("/http/", 7)) + "/json/sms/Report";
                    }
                    else
                    {
                        txtServerURL.Text = "http://www.dh3t.com/json/sms/Report";
                    }
                    break;
                default:
                    break;
            }
        }





        /**
       * 
       * 批量发送短信，选择调用方法
       * 
       * **/

        private void _brdo_Click(object sender, EventArgs e)
        {
            RadioButton _rdo = sender as RadioButton;
            opTag = _rdo.Tag.ToString();
            String _str = this._btxtServerURL.Text.Trim().ToString();
            Boolean _isBlank = "".Equals(_str);

            int _idxHttp = -1;
            if (_str.Length > 7)
            {
                _idxHttp = _str.IndexOf("/http/", 7);
            }

            switch (opTag)
            {
                case "SendSMS"://发送短信
                    this._btxtPhones1.Enabled = true;
                    this._btxtPhones2.Enabled = true;

                    this._btxtContent1.Enabled = true;
                    this._btxtContent2.Enabled = true;

                    this._btxtSmsId1.Enabled = true;
                    this._btxtSmsId2.Enabled = true;

                    this._btxtSign1.Enabled = true;
                    this._btxtSign2.Enabled = true;

                    this._btxtSubcode1.Enabled = true;
                    this._btxtSubcode2.Enabled = true;

                    if ("".Equals(this._btxtSmsId1.Text.Trim()))
                    {
                        this._btxtSmsId1.Text = Guid.NewGuid().ToString().Replace("-", "");

                    }
                      if ("".Equals(this._btxtSmsId2.Text.Trim()))
                    {
                        this._btxtSmsId2.Text = Guid.NewGuid().ToString().Replace("-", "");

                    }
                    if (!_isBlank && _idxHttp > -1)
                    {
                        this._btxtServerURL.Text = _str.Substring(0, _str.IndexOf("/http/", 7)) + "/json/sms/BatchSubmit";
                    }
                    else
                    {
                        this._btxtServerURL.Text = "http://www.dh3t.com/json/sms/BatchSubmit";
                    }
                    break;
                case "GetSMS"://获取上行
                    this._btxtPhones1.Enabled = false;
                    this._btxtPhones2.Enabled = false;

                    this._btxtContent1.Enabled = false;
                    this._btxtContent2.Enabled = false;

                    this._btxtSmsId1.Enabled = false;
                    this._btxtSmsId2.Enabled = false;

                    this._btxtSign1.Enabled = false;
                    this._btxtSign2.Enabled = false;

                    this._btxtSubcode1.Enabled = false;
                    this._btxtSubcode2.Enabled = false;

                    if (!_isBlank && _idxHttp > -1)
                    {
                        this._btxtServerURL.Text = _str.Substring(0, _str.IndexOf("/http/", 7)) + "/json/sms/Deliver";
                    }
                    else
                    {
                        this._btxtServerURL.Text = "http://www.dh3t.com/json/sms/Deliver";
                    }
                    break;

                case "GetReport"://获取状态报告
                     this._btxtPhones1.Enabled = false;
                     this._btxtPhones2.Enabled = false;

                     this._btxtContent1.Enabled = false;
                     this._btxtContent2.Enabled = false;

                     this._btxtSmsId1.Enabled = false;
                    this._btxtSmsId2.Enabled = false;

                    this._btxtSign1.Enabled = false;
                    this._btxtSign2.Enabled = false;

                    this._btxtSubcode1.Enabled = false;
                    this._btxtSubcode2.Enabled = false;

                    if (!_isBlank && _idxHttp > -1)
                    {
                        this._btxtServerURL.Text = _str.Substring(0, _str.IndexOf("/http/", 7)) + "/json/sms/Report";
                    }
                    else
                    {
                        this._btxtServerURL.Text = "http://www.dh3t.com/json/sms/Report";
                    }
                    break;
                default:
                    break;
            }
        }


        /**
  * 
  * 生成普通发送短信的JSON 请求数据包
  * 
  * 
  * **/
        private String packageSendSmsJsonData(String account, String passwd,SendSmsData sendSmsData)
        {
            String data = "{\"account\":\"" + account + "\""
                        + ",\"password\":\"" + passwd + "\""
                        + ",\"msgid\":\"" + sendSmsData.Msgid + "\""
                        + ",\"phones\":\"" + sendSmsData.Phones + "\""
                        + ",\"content\":\"" + sendSmsData.Content + "\""
                        + ",\"sign\":\"" + sendSmsData.Sign + "\""
                        + ",\"subcode\":\"" + sendSmsData.Subcode + "\""
                        + "}";

            return data;
        }




        private String packageBatchSendSmsJsonData(String account, String passwd, List<SendSmsData> list)
        {
            String data = "{\"account\":\"" + account + "\""
                            + ",\"password\":\"" + passwd + "\""
                            + ",\"data\":[";

             for (int i = 0; i < list.Count; i++) {
                SendSmsData sendSmsData= list[i];
                data += "{"
                       + "\"msgid\":\"" + sendSmsData.Msgid + "\""
                       + ",\"phones\":\"" + sendSmsData.Phones + "\""
                       + ",\"content\":\"" + sendSmsData.Content + "\""
                       + ",\"sign\":\"" + sendSmsData.Sign + "\""
                       + ",\"subcode\":\"" + sendSmsData.Subcode + "\""
                       + "}";

                    if (i < list.Count - 1) {
                       data += ",";
                
                     }
            
            
            }

          data += "]}";
            return data;
        
        }


        /**
       * 
       * 生成获取状态报告的JSON 请求数据包
       * 
       * **/
        private String packageReportJsonData(String account, String passwd)
        {
            String data = "{\"account\":\"" + account + "\""
                        + ",\"password\":\"" + passwd + "\""
                        + "}";

            return data;
        }

        /**
         * 
         * 生成获取上行回复的JSON 请求数据包
         * 
         * **/
        private String packageDeliverJsonData(String account, String passwd)
        {
            String data = "{\"account\":\"" + account + "\""
                        + ",\"password\":\"" + passwd + "\""
                        + "}";

            return data;
        }


        /**
         * 
         * Post 发送请求数据
         * 
         * 
         * **/
        private String  postMethodConnServer(String iServerURL, String iPostData)
        {
            String result = null;
            byte[] _buffer = Encoding.GetEncoding("utf-8").GetBytes(iPostData);
            HttpWebRequest _req = (HttpWebRequest)WebRequest.Create(iServerURL);
            _req.Method = "Post";
            _req.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
            _req.ContentLength = _buffer.Length;
            Stream _stream = null;
            Stream _resStream = null;
            StreamReader _resSR = null;
            try
            {
                _stream = _req.GetRequestStream();
                _stream.Write(_buffer, 0, _buffer.Length);
                _stream.Flush();
                HttpWebResponse _res = (HttpWebResponse)_req.GetResponse();

                //获取响应
                _resStream = _res.GetResponseStream();
                _resSR = new StreamReader(_resStream, Encoding.GetEncoding("utf-8"));
                 result = _resSR.ReadToEnd();
                //MessageBox.Show(_resSR.ReadToEnd());
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "调用异常", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            finally
            {
                if (_stream != null)
                {
                    _stream.Close();
                }
                if (_resSR != null)
                {
                    _resSR.Close();
                }
                if (_resStream != null)
                {
                    _resStream.Close();
                }
            }
            return result;
        }


        //MD5加密程序（32位小写）
        private static string md5(string str)
        {
            byte[] result = Encoding.Default.GetBytes(str);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash(result);
            String md = BitConverter.ToString(output).Replace("-", "");
            return md.ToLower();
        }

        private void frmHttpSend_Load(object sender, EventArgs e)
        {
            generalNewData();
        }

        private void generalNewData()
        {
            if (rdoSendSMS.Checked)
            {
                txtSmsId.Text = Guid.NewGuid().ToString().Replace("-", "");

            }
        }

        /**
         * 
         * 点击退出
         * **/
        private void _bexitBtn_Click(object sender, EventArgs e)
        {
            DialogResult _dr = MessageBox.Show("确定退出？", "操作提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
            if (_dr == DialogResult.Yes)
            {
                this.Close();
                this.Dispose();
                Application.Exit();
            }
        }


        /**
         * 
         * 批量发送短信
         * 
         * **/
        private void _bsubmitBtn_Click(object sender, EventArgs e)
        {

            this._bsubmitBtn.Enabled = false;
            this._bexitBtn.Enabled = false;
            if (checkBatchInput())
            {
                String _serverURL =this._btxtServerURL.Text.Trim();
                String _account = this._btxtUserName.Text.Trim();
                String _passWord = md5(this._btxtUserPswd.Text.Trim());
                String _data = null;
 
                 switch (opTag)
                {
                    case "SendSMS"://发送短信
                        String _sign = this.txtSign.Text.Trim();
                        String _subCode = this.txtSubCode.Text.Trim();
                        List<SendSmsData> sendList = new List<SendSmsData>();

                        SendSmsData sendSmsData1 = new SendSmsData();
                        sendSmsData1.Content = this._btxtContent1.Text.Trim();
                        sendSmsData1.Phones = this._btxtPhones1.Text.Trim();
                        sendSmsData1.Sign = this._btxtSign1.Text.Trim();
                        sendSmsData1.Msgid = this._btxtSmsId1.Text.Trim();
                        sendSmsData1.Subcode = this._btxtSubcode1.Text.Trim();

                        sendList.Add(sendSmsData1);

                        SendSmsData sendSmsData2 = new SendSmsData();
                        sendSmsData2.Content = this._btxtContent2.Text.Trim();
                        sendSmsData2.Phones = this._btxtPhones2.Text.Trim();
                        sendSmsData2.Sign = this._btxtSign2.Text.Trim();
                        sendSmsData2.Msgid = this._btxtSmsId2.Text.Trim();
                        sendSmsData2.Subcode = this._btxtSubcode2.Text.Trim();
                        sendList.Add(sendSmsData2);
                        _data = this.packageBatchSendSmsJsonData(_account, _passWord, sendList);
                        break;
                    case "GetSMS"://获取上
                        _data = this.packageDeliverJsonData(_account, _passWord);
                        break;
                    case "GetReport"://获取状态报告
                        _data = this.packageReportJsonData(_account, _passWord);
                        break;

                    default:
                        break;
                }
                 this._btxtPostData.Text = _data;
                this._btxtResponseData.Text=  postMethodConnServer(_serverURL, _data);
            }
            this._bsubmitBtn.Enabled = true;
            this._bexitBtn.Enabled = true;

        }



        /**
         * 效验批量发送接口输入框
         * 
         * **/

        private bool checkBatchInput()
        {
            if ("".Equals(this._btxtServerURL.Text.Trim()))
            {
                this._btxtServerURL.Focus();
                MessageBox.Show("请输入服务地址！", "输入提示");
                return false;
            }

            if ("".Equals(this._btxtUserName.Text.Trim()))
            {
                this._btxtUserName.Focus();
                MessageBox.Show("请输入账号！", "输入提示");
                return false;
            }

            if ("".Equals(this._btxtUserPswd.Text.Trim()))
            {
                this._btxtUserPswd.Focus();
                MessageBox.Show("请输入密码！", "输入提示");
                return false;
            }


            return true;
        }



        /**
         * 
         * 效验普通发送短信输入框
         * **/
        private bool checkInput()
        {
            if ("".Equals(txtServerURL.Text.Trim()))
            {
                txtServerURL.Focus();
                MessageBox.Show("请输入服务地址！", "输入提示");
                return false;
            }

            if ("".Equals(txtUserName.Text.Trim()))
            {
                txtUserName.Focus();
                MessageBox.Show("请输入账号！", "输入提示");
                return false;
            }

            if ("".Equals(txtPassWord.Text.Trim()))
            {
                txtPassWord.Focus();
                MessageBox.Show("请输入密码！", "输入提示");
                return false;
            }

         

            return true;
        }
    }
}
