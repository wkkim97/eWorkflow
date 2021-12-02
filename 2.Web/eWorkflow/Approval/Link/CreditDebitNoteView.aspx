<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Popup.master" AutoEventWireup="true" CodeFile="CreditDebitNoteView.aspx.cs" Inherits="Approval_Link_CreditDebitNoteView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="Server">
    <style type="text/css">
        html, body, div, span {
            font-family: Helvetica;
            font-size: 10pt;
        }

        table.bankInfo {
            font-size: 9pt;
            width: 100%;
            border: none;
        }

            table.bankInfo th {
                text-align: left;
            }

        .MoneyDisplay {
            font-size: 13pt;
            float: right;
            text-align: right;
        }

        .MoneyDisplayBold {
            font-size: 13pt;
            float: right;
            text-align: right;
            font-weight: bold;
        }

        .labelBold {
            font-weight: bold;
        }

        hr {
            display: block;
            height: 5px;
            background: transparent;
            border: none;
            border-top: solid 5px rgb(103,103,103);
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HolderBody" runat="Server">
    <div style="width: 99%; margin: 10px 3px 10px 3px;">
        <table style="width: 100%;">
            <tr>
                <td style="width: 50%" colspan="2">
                    <b style="font-size: xx-large">Invoice</b>
                    <br />
                    &nbsp;&nbsp<asp:Label ID="lblType" runat="server" Text="Debit note"></asp:Label>
                    <br />
                    <asp:Label ID="lblDocNo" runat="server" Text="" CssClass="labelBold"></asp:Label>
                </td>
                <td style="width: 50%; text-align: right; padding-top: 20px; padding-right: 70px">
                    <img src="/eWorks/Styles/images/bayer_large.png" alt="bayer">
                </td>
            </tr>
            <tr>
                <td style="vertical-align: top; width: 20px"><b>To:</b></td>
                <td style="vertical-align: top">
                    <asp:Label ID="lblToAddress" runat="server"></asp:Label>
                </td>
                <td>
                    <div id="divCompany" runat="server" style="width: 100%">
                        <asp:Label ID="lblCompanAddress" runat="server"></asp:Label>
                    </div>
                    <div id="divPerson" runat="server" style="margin-top: 5px; font-weight: bold">
                        ACCT.:KangIl Lee<br />
                        Telephone :82 2 829 6765<br />
                        Telefax : 82 2 843 2976<br />
                        E-Mail : kangil.lee@bayer.com
                    </div>
                </td>
            </tr>
        </table>
        <br />
        
        <table style="width: 100%">
            <tr>
                <td style="vertical-align: top; width: 10px">&nbsp;</td>
                <td style="width: 70%">
                    <asp:Label ID="lblAttn" runat="server" CssClass="labelBold"></asp:Label>
                </td>
                <td style="width: 30%"><span style="display: inline-block; width: 90px; text-align: right;">Invoice Date : </span>
                    <asp:Label ID="lblInvoiceDate" runat="server" style="font-size:12px"></asp:Label><br />
                </td>
            </tr>
            <tr>
                <td style="vertical-align: top; width: 10px">&nbsp;</td>
                <td>
                    <asp:Label ID="lblCc" runat="server" CssClass="labelBold"></asp:Label>
                </td>
                <td>
                    <span style="display: inline-block; width: 90px; text-align: right; font-weight: bold">Due Date :</span>
                    <asp:Label ID="lblDueDate" runat="server" style="font-size:12px"></asp:Label>
                </td>
            </tr>
        </table>
        <br />
        <hr />
        &nbsp;&nbsp;&nbsp;We hereby invoice you for
        <div id="divItem" runat="server">

            <%--        <table style="width: 100%">
            <tr>
                <td style="">We hereby invoice you for<br />
                    R&D for September 2014 Actual</td>
                <td style="width: 200px"></td>
                <td style="width: 150px">
                    <span style="font-size: 13pt; display: inline-block; width: 120px; float: right">
                        <asp:Label ID="lblAmount" runat="server" CssClass="MoneyDisplay">0</asp:Label></span><br />
                </td>
                <td style="width: 60px"></td>
            </tr>
        </table>--%>
        </div>
        <table style="width: 100%">
            <colgroup>
                <col style="width: 3px;" />
                <col />
                <col style="width: 150px;" />
                <col style="width: 120px;" />
            </colgroup>

            <tr>
                <td></td>
                <td>
                    <span style="font-size: 13pt; display: inline-block; width: 180px; text-align: right">Total Amount</span>

                </td>
                <td>
                    <span style="font-size: 13pt; display: inline-block; width: 120px; float: right">
                        <asp:Label ID="lblTotalAmount" runat="server" CssClass="MoneyDisplayBold">0</asp:Label></span>
                </td>
                <td>
                    <asp:Label ID="lblCurrency" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <span style="font-size: 13pt; display: inline-block; width: 180px; text-align: right;">Local Amount(KRW)</span>

                </td>
                <td>
                    <span style="font-size: 13pt; display: inline-block; width: 120px; float: right">
                        <asp:Label ID="lblLocalAmount" runat="server" CssClass="MoneyDisplayBold">0</asp:Label></span>
                </td>
                <td></td>
            </tr>
        </table>

        <hr />
        <p style="font-size: 9pt">If you need to wire transfer the amount mentioned above instead of BIC, please refer to the bank information below.</p>
        <br />
        <div id="divBKL" runat="server" style="width: 100%">
            <table class="bankInfo">
                <tr>
                    <th style=""><u>Bank</u></th>
                    <th style="width: 120px"><u>Currency</u></th>
                    <th style="width: 130px"><u>A/C No.</u></th>
                    <th style="width: 120px"><u>Swift Code</u></th>
                </tr>
                <tr>
                    <td>Citi Bank, N.A.</td>
                    <td>USD</td>
                    <td>5 - 001545 - 002</td>
                    <td>CITIKRSX</td>
                </tr>
                <tr>
                    <td>Citicorp Center	</td>
                    <td>EUR</td>
                    <td>5 - 001545 - 096</td>
                    <td>CITIKRSX</td>
                </tr>
                <tr>
                    <td>89-29, 2-ka, Shinmoon-ro, Chongro-ku, Seoul</td>
                    <td>KRW</td>
                    <td>0 - 001545 - 019</td>
                    <td>CITIKRSX</td>
                </tr>
                <tr>
                    <td>Beneficiary : Bayer Korea Ltd</td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>
            </table>
        </div>
        <div id="divBCS" runat="server" style="width: 100%">
            <table class="bankInfo">
                <tr>
                    <th style=""><u>Bank</u></th>
                    <th style="width: 120px"><u>Currency</u></th>
                    <th style="width: 130px"><u>A/C No.</u></th>
                    <th style="width: 120px"><u>Swift Code</u></th>
                </tr>
                <tr>
                    <td>Citi Bank, N.A.</td>
                    <td>USD</td>
                    <td>5 - 001648 - 014</td>
                    <td>CITIKRSX</td>
                </tr>
                <tr>
                    <td>Citicorp Center	</td>
                    <td>JPY</td>
                    <td>5 - 001648 - 049</td>
                    <td>CITIKRSX</td>
                </tr>
                <tr>
                    <td>89-29, 2-ka, Shinmoon-ro, Chongro-ku, Seoul</td>
                    <td>EUR</td>
                    <td>5 - 001648 - 022</td>
                    <td>CITIKRSX</td>
                </tr>
                <tr>
                    <td>Beneficiary : Bayer CropScience Ltd.</td>
                    <td>KRW</td>
                    <td>0 - 001648 - 004</td>
                    <td>CITIKRSX</td>
                </tr>
            </table>
        </div>
        <div id="divBMSG" runat="server" style="width: 100%">
            <table class="bankInfo">
                <tr>
                    <th style=""><u>Bank</u></th>
                    <th style="width: 120px"><u>Currency</u></th>
                    <th style="width: 130px"><u>A/C No.</u></th>
                    <th style="width: 120px"><u>Swift Code</u></th>
                </tr>
                <tr>
<%--                    <td>Citibank Korea Inc..</td>
                    <td>USD</td>
                    <td>5-001567-057</td>
                    <td>CITIKRSX</td>--%>
                </tr>
                <tr>
<%--                    <td>39, Da-dong, JoongGu, Seoul 110-180, Korea</td>
                    <td>EUR</td>
                    <td>5-001567-049</td>
                    <td>CITIKRSX</td>
                </tr>--%>
                <tr>
                    <td>Beneficiary : Bayer Material Science Ltd</td>
                    <td>KRW</td>
                    <td>0-001567-004</td>
                    <td>CITIKRSX</td>
                </tr>
            </table>
        </div>
        <div style="width: 600px; padding-top: 20px; text-align: center;">
            <img src="../../Styles/images/Common/CreditDebitSign.png" width="600"></img>
        </div>
    </div>
</asp:Content>

