<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Popup.master" AutoEventWireup="true" CodeFile="BusinessCardView.aspx.cs" Inherits="Approval_Link_BusinessCardView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" runat="Server">
    <style type="text/css">
        .scroll{
            overflow-x:hidden;
            overflow-y:hidden;
        }
        *{color:#10384F; font-size:13px}
        .title-margin{
            margin-right:20px;
            margin-top:5px;
            float:right;
            font-weight:900;
            font-size:12px

        }
        .title-margine{
            margin-top:5px;
            float:right;
            font-weight:900;
            font-size:12px;
            
            

        }
        .title-name{
            float:right;
            letter-spacing:20px;
            font-size:25px;
        }
        
        .font-blue{
            color:#0091DF !important;
            letter-spacing:20px;
            float:right;
            font-size:30px;
        }
        .font-raspberry{
            color:#D30F4B !important;
            letter-spacing:20px;
            float:right;
            font-size:30px;
        }
        .font-midpurple{
            color:#624963 !important;
            letter-spacing:20px;
            float:right;
             font-size:30px;
        }
         .font-green{
            color:#66B512  !important;
            letter-spacing:20px;
            float:right;
             font-size:30px;
        }
         


        .font-raspberrye{
            color:#D30F4B !important;
            float:right;
            font-style:italic;
            font-size:25px;
            font-weight:100;
        }
        .font-midpurplee{
            color:#624963 !important;
            float:right;
            font-style:italic;
            font-size:25px;
        }
         .font-greene{
            color:#66B512  !important;            
            float:right;
            font-style:italic;
            font-size:25px;
        }
          .font-bluee{
            color:#0091DF !important;
            
            float:right;
            font-style:italic;
            font-size:25px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HolderBody" runat="Server">
    <span style="color:red" runat="Server"> 최종 시안은 업체 별도 송부용으로 확정 바랍니다</span>
    <div id="divKorNameCard" runat="server" class="scroll" style="display: block; border: 1px solid black; width: 410px; height: 260px; margin: 30px 30px; position: relative; top: 0px; font-family: 'New Gulim' Arial, Helvetica, sans-serif;">
        <div id="divBayerLogo1" runat="server" style="text-align: right; position: absolute; top: 25px; right: 30px">
            <img src="../../Styles/images/bayer_large.png" />
        </div>
        <div id="divNameCardList1" style="margin: 30px 0px 5px 10px; line-height: 1.1em;" runat="server">
           
            <table>
                <!--
                <tr>
                    <td><div id="logoImgArea" runat="server" ></div></td>
                </tr>
                 <tr style="display:none">
                    <td><asp:Label ID="labelKorLogo" runat="server" Font-Bold="true"></asp:Label></td>
                </tr>
                -->
                <tr>
                    <td>
                         <asp:Label ID="labelKorCompany" runat="server" ></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="labelKorDivision" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="labelKorDepartment" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                         <asp:Label ID="labelKorAddress1" runat="server"></asp:Label><br />
                        <asp:Label ID="labelKorAddress2" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="labelKorTel" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="labelKorFax" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                       <asp:Label ID="labelKorMobile" runat="server"></asp:Label>
                    </td>
                </tr>
                 <tr>
                    <td>
                       <asp:Label ID="labelKorEmail" runat="server"></asp:Label>
                    </td>
                </tr>
                 <tr>
                    <td>
                       <asp:Label ID="labelKorWebAddress" runat="server"></asp:Label>
                    </td>
                </tr>


            </table>
            <br />
            <table style="margin: 20px 10px 0px 0px; float: right;">
                <tr>
                    <td >
                         <asp:Label ID="labelKorName" CssClass="title-name"  runat="server"></asp:Label><br />
                       
                    </td>
                </tr>
                <tr>
                    <td >
                        <asp:Label ID="labelKorTitle" CssClass="title-margin"   runat="server"></asp:Label>
                    </td>
                </tr>
               
            </table>
        </div>
    </div>

    <div id="divEngNameCard" runat="server" class="scroll" style="display: block; border: 1px solid black; width: 410px; height: 260px; margin: 30px 30px; position: relative; top: 0px; font-family: 'New Gulim' Arial, Helvetica, sans-serif">
        <div id="divBayerLogo2" runat="server" style="text-align: right; position: absolute; top: 25px; right: 30px">
            <img src="../../Styles/images/bayer_large.png" />
        </div>
        <div id="divNameCardList2" style="margin: 30px 0px 5px 10px; line-height: 1.1em;"  runat="server">
            <table style="height: 100%;">                
                <tr>
                    <td>
                        <asp:Label ID="labelEngCompany" runat="server" ></asp:Label>
                        
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="labelEngDivision" runat="server"></asp:Label>
                        
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="labelEngDepartment" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="labelEngAddress1" runat="server"></asp:Label><br />
                        <asp:Label ID="labelEngAddress2" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="labelEngTel" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="labelEngFax" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="labelEngMobile" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="labelEngEmail" runat="server"></asp:Label>
                    </td>
                </tr>
                 <tr>
                    <td>
                        <asp:Label ID="labelEngWebAddress" runat="server"></asp:Label>
                    </td>
                </tr>
                 
                </table>
            <br />
            <table style="margin: 20px 10px 0px 0px; float: right;">
                <tr>
                    <td >
                         <asp:Label ID="labelEngName" CssClass="title-name"  runat="server" Font-Size="25px"></asp:Label><br />
                       
                    </td>
                </tr>
                <tr>
                    <td >
                        <asp:Label ID="labelEngTitle" CssClass="title-margine"   runat="server"></asp:Label>
                    </td>
                </tr>
               
            </table>
        </div>
    </div>
    <div id="hiddenArea">
        <input type="hidden" id="hddProcessId" runat="server" />
    </div>
</asp:Content>

