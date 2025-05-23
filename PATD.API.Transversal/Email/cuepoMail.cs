﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PATD.API.Transversal.Email
{
    public class IniciarSesion
    {
        string body = @"<!DOCTYPE HTML PUBLIC '-//W3C//DTD XHTML 1.0 Transitional //EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>
                            <html xmlns='http://www.w3.org/1999/xhtml' xmlns:v='urn:schemas-microsoft-com:vml'
                              xmlns:o='urn:schemas-microsoft-com:office:office'>

                            <head>
                              <!--[if gte mso 9]>
                            <xml>
                              <o:OfficeDocumentSettings>
                                <o:AllowPNG/>
                                <o:PixelsPerInch>96</o:PixelsPerInch>
                              </o:OfficeDocumentSettings>
                            </xml>
                            <![endif]-->
                              <meta http-equiv='Content-Type' content='text/html; charset=UTF-8'>
                              <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                              <meta name='x-apple-disable-message-reformatting'>
                              <!--[if !mso]><!-->
                              <meta http-equiv='X-UA-Compatible' content='IE=edge'>
                              <!--<![endif]-->
                              <title></title>

                              <style type='text/css'>
                                table,
                                td {
                                  color: #000000;
                                }

                                @media only screen and (min-width: 620px) {
                                  .u-row {
                                    width: 600px !important;
                                  }

                                  .u-row .u-col {
                                    vertical-align: top;
                                  }

                                  .u-row .u-col-100 {
                                    width: 600px !important;
                                  }

                                }

                                @media (max-width: 620px) {
                                  .u-row-container {
                                    max-width: 100% !important;
                                    padding-left: 0px !important;
                                    padding-right: 0px !important;
                                  }

                                  .u-row .u-col {
                                    min-width: 320px !important;
                                    max-width: 100% !important;
                                    display: block !important;
                                  }

                                  .u-row {
                                    width: calc(100% - 40px) !important;
                                  }

                                  .u-col {
                                    width: 100% !important;
                                  }

                                  .u-col>div {
                                    margin: 0 auto;
                                  }
                                }

                                body {
                                  margin: 0;
                                  padding: 0;
                                }

                                table,
                                tr,
                                td {
                                  vertical-align: top;
                                  border-collapse: collapse;
                                }

                                p {
                                  margin: 0;
                                }

                                .ie-container table,
                                .mso-container table {
                                  table-layout: fixed;
                                }

                                * {
                                  line-height: inherit;
                                }

                                a[x-apple-data-detectors='true'] {
                                  color: inherit !important;
                                  text-decoration: none !important;
                                }
                              </style>



                              <!--[if !mso]><!-->
                              <link href='https://fonts.googleapis.com/css?family=Lato:400,700&display=swap' rel='stylesheet' type='text/css'>
                              <!--<![endif]-->

                            </head>

                            <body class='clean-body'
                              style='margin: 0;padding: 0;-webkit-text-size-adjust: 100%;background-color: #f9f9f9;color: #000000'>
                              <!--[if IE]><div class='ie-container'><![endif]-->
                              <!--[if mso]><div class='mso-container'><![endif]-->
                              <table
                                style='border-collapse: collapse;table-layout: fixed;border-spacing: 0;mso-table-lspace: 0pt;mso-table-rspace: 0pt;vertical-align: top;min-width: 320px;Margin: 0 auto;background-color: #f9f9f9;width:100%'
                                cellpadding='0' cellspacing='0'>
                                <tbody>
                                  <tr style='vertical-align: top'>
                                    <td style='word-break: break-word;border-collapse: collapse !important;vertical-align: top'>
                                      <!--[if (mso)|(IE)]><table width='100%' cellpadding='0' cellspacing='0' border='0'><tr><td align='center' style='background-color: #f9f9f9;'><![endif]-->


                                      <div class='u-row-container' style='padding: 0px;background-color: transparent'>
                                        <div class='u-row'
                                          style='Margin: 0 auto;min-width: 320px;max-width: 600px;overflow-wrap: break-word;word-wrap: break-word;word-break: break-word;background-color: transparent;'>
                                          <div style='border-collapse: collapse;display: table;width: 100%;background-color: transparent;'>
                                            <!--[if (mso)|(IE)]><table width='100%' cellpadding='0' cellspacing='0' border='0'><tr><td style='padding: 0px;background-color: transparent;' align='center'><table cellpadding='0' cellspacing='0' border='0' style='width:600px;'><tr style='background-color: transparent;'><![endif]-->

                                            <!--[if (mso)|(IE)]><td align='center' width='600' style='background-color: #0083c6;width: 600px;padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;' valign='top'><![endif]-->
                                            <div class='u-col u-col-100'
                                              style='max-width: 320px;min-width: 600px;display: table-cell;vertical-align: top;'>
                                              <div style='background-color: #0083c6;width: 100% !important;'>
                                                <!--[if (!mso)&(!IE)]><!-->
                                                <div
                                                  style='padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;'>
                                                  <!--<![endif]-->

                                                  <!--[if (!mso)&(!IE)]><!-->
                                                </div>
                                                <!--<![endif]-->
                                              </div>
                                            </div>
                                            <!--[if (mso)|(IE)]></td><![endif]-->
                                            <!--[if (mso)|(IE)]></tr></table></td></tr></table><![endif]-->
                                          </div>
                                        </div>
                                      </div>



                                      <div class='u-row-container' style='padding: 0px;background-color: transparent'>
                                        <div class='u-row'
                                          style='Margin: 0 auto;min-width: 320px;max-width: 600px;overflow-wrap: break-word;word-wrap: break-word;word-break: break-word;background-color: #ffffff;'>
                                          <div style='border-collapse: collapse;display: table;width: 100%;background-color: transparent;'>
                                            <!--[if (mso)|(IE)]><table width='100%' cellpadding='0' cellspacing='0' border='0'><tr><td style='padding: 0px;background-color: transparent;' align='center'><table cellpadding='0' cellspacing='0' border='0' style='width:600px;'><tr style='background-color: #ffffff;'><![endif]-->

                                            <!--[if (mso)|(IE)]><td align='center' width='600' style='width: 600px;padding: 0px 0px 20px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;' valign='top'><![endif]-->
                                            <div class='u-col u-col-100'
                                              style='max-width: 320px;min-width: 600px;display: table-cell;vertical-align: top;'>
                                              <div style='width: 100% !important;'>
                                                <!--[if (!mso)&(!IE)]><!-->
                                                <div
                                                  style='padding: 0px 0px 20px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;'>
                                                  <!--<![endif]-->

                                                  <table style='font-family:'Lato',sans-serif;' role='presentation' cellpadding='0' cellspacing='0'
                                                    width='100%' border='0'>
                                                    <tbody>
                                                      <tr>
                                                        <td
                                                          style='overflow-wrap:break-word;word-break:break-word;padding:40px 40px 0px;font-family:'Lato',sans-serif;'
                                                          align='left'>

                                                          <div style='line-height: 140%; text-align: left; word-wrap: break-word;'>
                                                            <p style='font-size: 14px; line-height: 140%;'><span
                                                                style='font-size: 18px; line-height: 25.2px; color: #000000; font-family: arial, helvetica, sans-serif;'>Hola
                                                                {0},</span></p>
                                                            <p style='font-size: 14px; line-height: 140%;'>&nbsp;</p>
                                                            <p style='font-size: 14px; line-height: 140%;'>{1}</p>
                                                            <p style='font-size: 14px; line-height: 140%;'>&nbsp;</p>
                                                            <p style='font-size: 14px; line-height: 140%;'>Si tiene alguna duda, llame al +504 2276
                                                              8145</p>
                                                            <p style='font-size: 14px; line-height: 140%; text-align: justify;'>&nbsp;</p>
                                                          </div>

                                                        </td>
                                                      </tr>
                                                    </tbody>
                                                  </table>

                                                  <!--[if (!mso)&(!IE)]><!-->
                                                </div>
                                                <!--<![endif]-->
                                              </div>
                                            </div>
                                            <!--[if (mso)|(IE)]></td><![endif]-->
                                            <!--[if (mso)|(IE)]></tr></table></td></tr></table><![endif]-->
                                          </div>
                                        </div>
                                      </div>



                                      <div class='u-row-container' style='padding: 0px;background-color: #f9f9f9'>
                                        <div class='u-row'
                                          style='Margin: 0 auto;min-width: 320px;max-width: 600px;overflow-wrap: break-word;word-wrap: break-word;word-break: break-word;background-color: #1c103b;'>
                                          <div style='border-collapse: collapse;display: table;width: 100%;background-color: transparent;'>
                                            <!--[if (mso)|(IE)]><table width='100%' cellpadding='0' cellspacing='0' border='0'><tr><td style='padding: 0px;background-color: #f9f9f9;' align='center'><table cellpadding='0' cellspacing='0' border='0' style='width:600px;'><tr style='background-color: #1c103b;'><![endif]-->

                                            <!--[if (mso)|(IE)]><td align='center' width='600' style='background-color: #ff8c1c;width: 600px;padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;' valign='top'><![endif]-->
                                            <div class='u-col u-col-100'
                                              style='max-width: 320px;min-width: 600px;display: table-cell;vertical-align: top;'>
                                              <div style='background-color: #ff8c1c;width: 100% !important;'>
                                                <!--[if (!mso)&(!IE)]><!-->
                                                <div
                                                  style='padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;'>
                                                  <!--<![endif]-->

                                                  <table style='font-family:'Lato',sans-serif;' role='presentation' cellpadding='0' cellspacing='0'
                                                    width='100%' border='0'>
                                                    <tbody>
                                                      <tr>
                                                        <td
                                                          style='overflow-wrap:break-word;word-break:break-word;padding:10px;font-family:'Lato',sans-serif;'
                                                          align='left'>

                                                          <table height='0px' align='center' border='0' cellpadding='0' cellspacing='0' width='100%'
                                                            style='border-collapse: collapse;table-layout: fixed;border-spacing: 0;mso-table-lspace: 0pt;mso-table-rspace: 0pt;vertical-align: top;border-top: 1px solid #ff8c1c;-ms-text-size-adjust: 100%;-webkit-text-size-adjust: 100%'>
                                                            <tbody>
                                                              <tr style='vertical-align: top'>
                                                                <td
                                                                  style='word-break: break-word;border-collapse: collapse !important;vertical-align: top;font-size: 0px;line-height: 0px;mso-line-height-rule: exactly;-ms-text-size-adjust: 100%;-webkit-text-size-adjust: 100%'>
                                                                  <span>&#160;</span>
                                                                </td>
                                                              </tr>
                                                            </tbody>
                                                          </table>

                                                        </td>
                                                      </tr>
                                                    </tbody>
                                                  </table>

                                                  <!--[if (!mso)&(!IE)]><!-->
                                                </div>
                                                <!--<![endif]-->
                                              </div>
                                            </div>
                                            <!--[if (mso)|(IE)]></td><![endif]-->
                                            <!--[if (mso)|(IE)]></tr></table></td></tr></table><![endif]-->
                                          </div>
                                        </div>
                                      </div>



                                      <div class='u-row-container' style='padding: 0px;background-color: transparent'>
                                        <div class='u-row'
                                          style='Margin: 0 auto;min-width: 320px;max-width: 600px;overflow-wrap: break-word;word-wrap: break-word;word-break: break-word;background-color: #f9f9f9;'>
                                          <div style='border-collapse: collapse;display: table;width: 100%;background-color: transparent;'>
                                            <!--[if (mso)|(IE)]><table width='100%' cellpadding='0' cellspacing='0' border='0'><tr><td style='padding: 0px;background-color: transparent;' align='center'><table cellpadding='0' cellspacing='0' border='0' style='width:600px;'><tr style='background-color: #f9f9f9;'><![endif]-->

                                            <!--[if (mso)|(IE)]><td align='center' width='600' style='width: 600px;padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;' valign='top'><![endif]-->
                                            <div class='u-col u-col-100'
                                              style='max-width: 320px;min-width: 600px;display: table-cell;vertical-align: top;'>
                                              <div style='width: 100% !important;'>
                                                <!--[if (!mso)&(!IE)]><!-->
                                                <div
                                                  style='padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;'>
                                                  <!--<![endif]-->

                                                  <table style='font-family:'Lato',sans-serif;' role='presentation' cellpadding='0' cellspacing='0'
                                                    width='100%' border='0'>
                                                    <tbody>
                                                      <tr>
                                                        <td
                                                          style='overflow-wrap:break-word;word-break:break-word;padding:0px 40px 30px 20px;font-family:'Lato',sans-serif;'
                                                          align='left'>

                                                          <div style='line-height: 140%; text-align: left; word-wrap: break-word;'>

                                                          </div>

                                                        </td>
                                                      </tr>
                                                    </tbody>
                                                  </table>

                                                  <!--[if (!mso)&(!IE)]><!-->
                                                </div>
                                                <!--<![endif]-->
                                              </div>
                                            </div>
                                            <!--[if (mso)|(IE)]></td><![endif]-->
                                            <!--[if (mso)|(IE)]></tr></table></td></tr></table><![endif]-->
                                          </div>
                                        </div>
                                      </div>


                                      <!--[if (mso)|(IE)]></td></tr></table><![endif]-->
                                    </td>
                                  </tr>
                                </tbody>
                              </table>
                              <!--[if mso]></div><![endif]-->
                              <!--[if IE]></div><![endif]-->
                            </body>

                            </html>";

        public string InicioSesionEmail(string nombreCliente, string mensaje)
        {
            body =  body.Replace("{0}", nombreCliente);
            body =  body.Replace("{1}", mensaje);
            return body;
        }
    }
}
