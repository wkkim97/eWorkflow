<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ConfigConditionBox.ascx.cs" Inherits="Approval.Common.ConfigConditionBox" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<%--<ul style="margin-top: 1px; margin-bottom: 1px">--%>
<table>
    <tr>
        <td style="width: 150px">
            <telerik:RadTextBox runat="server" ID="radTxtField"></telerik:RadTextBox>
        </td>
        <td style="width: 100%">
            <telerik:RadDropDownList runat="server" ID="radComboCondition" Width="100%">
                <Items>
                    <telerik:DropDownListItem Text="=" Value="Equels" />
                    <telerik:DropDownListItem Text=">" Value="GreaterThan" />
                    <telerik:DropDownListItem Text="<" Value="LessThan" />
                    <telerik:DropDownListItem Text="StartWith" Value="StartWith" />
                    <telerik:DropDownListItem Text="NotStartWith" Value="NotStartWith" />
                    <telerik:DropDownListItem Text="Include" Value="Inclue" />
                    <telerik:DropDownListItem Text="NotInclude" Value="NotInclude" />
                    <telerik:DropDownListItem Text="IsNull" Value="IsNull" />
                    <telerik:DropDownListItem Text="IsNotNull" Value="IsNotNull" />
                </Items>
            </telerik:RadDropDownList>
        </td>
        <td style="width: 150px">
            <telerik:RadTextBox runat="server" ID="radTxtValue"></telerik:RadTextBox>
        </td>
        <td style="width: 100px">
            <telerik:RadDropDownList runat="server" ID="radComboOption">
                <Items>
                    <telerik:DropDownListItem Text="And" Value="And" Selected="true" />
                    <telerik:DropDownListItem Text="Or" Value="Or" />
                </Items>
            </telerik:RadDropDownList>
        </td>
        <td style="width: 40px">
            <telerik:RadButton runat="server" ID="radBtnDelete" Text="X" OnClick="radBtnDelete_Click"></telerik:RadButton>
        </td>
    </tr>
</table>
<%--</ul>--%>
