<%@ Page Title="" Language="C#" MasterPageFile="~/Master/eWorks_Popup.master" AutoEventWireup="true" CodeFile="Request.aspx.cs" Inherits="Approval_Process_Request"  %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Scripthead" Runat="Server">
    <script src="/eWorks/Scripts/Common/InterFaseService.js"></script>
    <script src="/eWorks/Scripts/Common/lyncStatus.js"></script>

    <script type="text/javascript">
        function fn_CancelClick(sender, args)
        {
            var oArg = new Object();
            oArg.returnValue = false;
            var oWnd = GetRadWindow();

            if (oArg.returnValue) {
                oWnd.close(oArg);
            }
            else
                oWnd.close();
        }

        $(document).ready(function () {
            try {
                $("[id^=pre_users]").each(function (i) {

                    if (_nameCtrl.PresenceEnabled) {
                        _nameCtrl.OnStatusChange = onStatusChange;
                        var userAddress = $(this).attr("value");
                        var userId = $(this).attr("userid");

                        //if (userAddress.startsWith('a'))
                        //    userAddress = 'loki-park@dotnetsoft.co.kr';
                        //else if (userAddress.startsWith('b'))
                        //    userAddress = 'cypher@dotnetsoft.co.kr';
                        //else if (userAddress.startsWith('w'))
                        //    userAddress = 'jaewoos@dotnetsoft.co.kr';
                        //else if (userAddress.startsWith('j'))
                        //    userAddress = 'soo@dotnetsoft.co.kr';
                        //else if (userAddress.startsWith('y'))
                        //    userAddress = 'amsmus@dotnetsoft.co.kr';
                        //else
                        //    userAddress = 'zest1116@dotnetsoft.co.kr';
                        var status = _nameCtrl.GetStatus(userAddress, userId);

                        $telerik.$($(this)).prepend("<span class='lync_status' id='pre_" + userId + "' onmouseover=ShowOOUI('" + userAddress + "') onmouseout=HideOOUI('" + userAddress + "') />")
                        $(this).bind("mouseover", function (e) {
                            _nameCtrl.ShowOOUI(userAddress, 0, e.offsetX - 10, e.offsetY - 10);
                            //ShowOOUI(userAddress);
                        });
                        $(this).bind("mouseout", function () { HideOOUI(userAddress); });

                    }

                });
            }
            catch (ex) { }
        });

        function FormLoad() {
            // 부모창에서 목록 가져옴.
            var files = window.parent.document.all.hddXmlFiles.value;
            $("[id$=hddXmlFiles]").val(files);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HolderBody" Runat="Server">
    <div class="align_left">
        <telerik:RadButton ID="btnRequest" CssClass="btn btn-blue btn-size4 bold" ButtonType="LinkButton" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" OnClick="btnRequest_Click" Text="Request" runat="server"></telerik:RadButton>
        <telerik:RadButton ID="btnCancel" CssClass="btn btn-darkgray btn-size4 bold" ButtonType="LinkButton" OnClientClicked="fn_CancelClick"  EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false" Text="Cancel" runat="server"></telerik:RadButton>
    </div> 
    <div class="data_type1 pt10">
        <table>
            <tbody>
                <tr>
                    <th>comment</th>
                </tr>
                <tr>
                    <td>
                        <telerik:RadTextBox TextMode="MultiLine" Height="60" Width="100%" ID="txtComment" runat="server"></telerik:RadTextBox></td>
                </tr>
                <tr>
                    <th>Approval line</th>
                </tr>
                <tr>
                    <td >
                        <telerik:RadGrid ID="grdApprovalLIne" runat="server" AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" AllowFilteringByColumn="false" GridLines="None" Skin="EXGrid">
                            <MasterTableView ShowHeadersWhenNoRecords="true"  ItemStyle-Wrap="false">
                                <Columns>
                                    <telerik:GridBoundColumn UniqueName="IDX" DataField="IDX" HeaderStyle-Width="2%" ItemStyle-Width="2%" >
                                    </telerik:GridBoundColumn>
                                    <telerik:GridTemplateColumn HeaderText="Name">
                                        <ItemTemplate>
                                            <span id="pre_users_<%#Eval("USER_ID")%>" value="<%#Eval("MAIL_ADDRESS")%>" userid="<%#Eval("USER_ID")%>">&nbsp;<%#Eval("USER_NAME")%> 
                                            </span>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn HeaderText="Org Name" UniqueName="ORG_NAME" DataField="ORG_NAME" HeaderStyle-Width="50%" ItemStyle-Width="50%"  >
                                    </telerik:GridBoundColumn>
                                </Columns>
                            </MasterTableView>
                            <ClientSettings>
                            </ClientSettings>
                            <HeaderStyle HorizontalAlign="Left" />
                        </telerik:RadGrid>
                    </td>
                </tr>
                <tr>
                    <th>Action Taker (Recipient) </th>
                </tr>
                <tr>
                    <td>
                        <telerik:RadListView ID="viewRecipient" AllowMultiFieldSorting="false" AllowPaging="false" runat="server">
                            <ItemTemplate>
                                <span id="pre_users_<%#Eval("USER_ID")%>" value="<%#Eval("MAIL_ADDRESS")%>" userid="<%#Eval("USER_ID")%>"><%#Eval("USER_NAME")%> </span>
                            </ItemTemplate>
                        </telerik:RadListView>
                    </td>
                </tr>
                <tr>
                    <th>Informed (Reviewer)</th>
                </tr>
                <tr>
                    <td>
                        <telerik:RadListView ID="viewReviewer" AllowMultiFieldSorting="false" AllowPaging="false" runat="server">
                            <ItemTemplate>
                                <span id="pre_users_<%#Eval("USER_ID")%>" value="<%#Eval("MAIL_ADDRESS")%>" userid="<%#Eval("USER_ID")%>"><%#Eval("USER_NAME")%> </span>
                            </ItemTemplate>
                        </telerik:RadListView>
                    </td>
                </tr>
            </tbody>
        </table>

    </div>
    
    <input id="hddProcessID" type="hidden" runat="server" />
    <input id="hddDocumentID" type="hidden" runat="server" />
    <input id="hddDocumentName" type="hidden" runat="server" />
    <input id="hddSubject" type="hidden" runat="server" />
    <input id="hddFolderPath" type="hidden" runat="server" />
    <input id="hddXmlFiles" type="hidden" runat="server" />
     <input id="hddReqFiles" type="hidden" runat="server" />
    <input id="hddRejectedProcessId" type="hidden" runat="server" />
</asp:Content>

