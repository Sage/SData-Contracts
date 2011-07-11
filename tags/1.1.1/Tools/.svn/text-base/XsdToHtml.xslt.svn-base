<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:sme="http://schemas.sage.com/sdata/sme/2007">
  <xsl:output method="html" indent="yes" encoding="utf-8" />
  <xsl:variable name="lo" select="'abcdefghijklmnopqrstuvwxyz'" />
  <xsl:variable name="up" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'" />
  <xsl:template match="xs:schema">
    <html>
      <head>
        <title>
          <xsl:value-of select="@targetNamespace" />
        </title>
        <link href="http://interop.sage.com/daisy/resources/skins/sageBranding2009/css/daisy.css" type="text/css" rel="stylesheet" />
        <link href="http://interop.sage.com/daisy/resources/skins/sageBranding2009/css/basic.css" type="text/css" rel="stylesheet" />
      </head>
      <body>
        <a name="_top" />
        <div id="bodyContainer">
          <p />
          <h1>
            Contract <xsl:value-of select="@targetNamespace" /></h1>
          <p />
          <table class="greyBorder" print-width="100%">
            <tr>
              <th>Resource Kind</th>
              <th>Plural</th>
              <th>Compliance</th>
              <th>CanGet</th>
              <th>CanPost</th>
              <th>CanPut</th>
              <th>CanDelete</th>
              <th>isSyncSource</th>
              <th>isSyncTarget</th>
            </tr>
            <xsl:apply-templates select="xs:element" mode="entity" />
          </table>
        </div>
        <xsl:apply-templates select="xs:complexType" />
        <xsl:apply-templates select="xs:simpleType" />
      </body>
    </html>
  </xsl:template>
  <xsl:template match="xs:element" mode="entity">
    <tr>
      <td>
        <a href="#{@name}--type">
          <xsl:value-of select="@name" />
        </a>
      </td>
      <td>
        <xsl:value-of select="@sme:pluralName" />
      </td>
      <td>
        <xsl:value-of select="@sme:compliance" />
      </td>
      <td>
        <xsl:value-of select="@sme:canGet" /> 
      </td>
      <td>
        <xsl:value-of select="@sme:canPost" /> 
      </td>
      <td>
        <xsl:value-of select="@sme:canPut" /> 
      </td>
      <td>
        <xsl:value-of select="@sme:canDelete" /> 
      </td>
      <td>
        <xsl:value-of select="@sme:isSyncSource" /> 
      </td>
      <td>
        <xsl:value-of select="@sme:isSyncTarget" /> 
      </td>
    </tr>
  </xsl:template>
  <xsl:template match="xs:complexType[xs:all]">
    <hr />
    <a name="{@name}" />
    <div style="float: right; clear: none;">
      <a href="#_top">Top</a>  
    </div>
    <div id="bodyContainer">
      <p />
      <h1>
        Resource <xsl:value-of select="substring(@name, 0, string-length(@name) - 5)" /></h1>
      <p />
      <table class="greyBorder" print-width="100%">
        <tr>
          <th>Name</th>
          <th>Type</th>
          <th>Label</th>
          <th>Unique</th>
          <th>Nillable</th>
          <th>Extension</th>
          <th>Mandatory</th>
          <th>Relationship</th>
          <th>Documentation</th>
        </tr>
        <xsl:apply-templates select="xs:all/xs:element" mode="property" />
      </table>
    </div>
  </xsl:template>
  <xsl:template match="xs:element" mode="property">
    <tr>
      <td>
        <xsl:value-of select="@name" />
      </td>
      <td>
        <xsl:choose>
          <xsl:when test="starts-with(@type,'xs:')">
            <xsl:value-of select="substring(@type,4)" />
          </xsl:when>
          <xsl:when test="contains(@type,'--type') and contains(@type,'tns:')">
            <xsl:variable name="type" select="substring(@type,5,string-length(@type) - 10)" />
            <a href="#{$type}--type">
              <xsl:value-of select="$type" />
            </a>
          </xsl:when>
          <xsl:when test="contains(@type,'--type')">
            <xsl:variable name="type" select="substring(@type,1,string-length(@type) - 6)" />
            <a href="#{$type}--type">
              <xsl:value-of select="$type" />
            </a>
          </xsl:when>
          <xsl:when test="contains(@type,'--list') and contains(@type,'tns:')">
            <xsl:variable name="type" select="substring(@type,5,string-length(@type) - 10)" />
            <a href="#{$type}--type">
              <xsl:value-of select="$type" />
            </a>*
          </xsl:when>
          <xsl:when test="contains(@type,'--list')">
            <xsl:variable name="type" select="substring(@type,1,string-length(@type) - 6)" />
            <a href="#{$type}--type">
              <xsl:value-of select="$type" />
            </a>*
          </xsl:when>
          <xsl:when test="contains(@type,'--enum') and contains(@type,'tns:')">
            <xsl:variable name="type" select="substring(@type,5,string-length(@type) - 10)" />
            <a href="#{$type}--enum">
              <xsl:value-of select="$type" />
            </a>
          </xsl:when>
          <xsl:when test="contains(@type,'--enum')">
            <xsl:variable name="type" select="substring(@type,1,string-length(@type) - 6)" />
            <a href="#{$type}--enum">
              <xsl:value-of select="$type" />
            </a>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="@type" />
          </xsl:otherwise>
        </xsl:choose>
      </td>
      <td>
        <xsl:value-of select="@sme:label" />
      </td>
      <td>
        <xsl:choose>
          <xsl:when test="@sme:isUniqueKey = 'true'">
            X
          </xsl:when>
          <xsl:otherwise>
            
          </xsl:otherwise>
        </xsl:choose>
      </td>
      <td>
        <xsl:choose>
          <xsl:when test="@nillable = 'true'">
            X
          </xsl:when>
          <xsl:otherwise>
             
          </xsl:otherwise>
        </xsl:choose>
      </td>
      <td>
        <xsl:choose>
          <xsl:when test="@sme:isExtension = 'true'">
            X
          </xsl:when>
          <xsl:otherwise>
             
          </xsl:otherwise>
        </xsl:choose>
      </td>
      <td>
        <xsl:choose>
          <xsl:when test="@sme:isMandatory = 'true'">
            X
          </xsl:when>
          <xsl:otherwise>
             
          </xsl:otherwise>
        </xsl:choose>
      </td>
      <td>
        <xsl:value-of select="@sme:relationship" /> 
      </td>
      <td>
        <xsl:value-of select="xs:annotation/xs:documentation" /> 
      </td>
    </tr>
  </xsl:template>
  <xsl:template match="xs:simpleType[xs:restriction/xs:enumeration]">
    <hr />
    <div style="float: right; clear: none;">
      <a href="#_top">Top</a>  
    </div>
    <div id="bodyContainer">
      <p />
      <a name="{@name}" />
      <h1>
        Enumeration <xsl:value-of select="substring(@name, 0, string-length(@name) - 5)" /></h1>
      <p />
      <table class="greyBorder" print-width="100%">
        <xsl:for-each select="xs:restriction/xs:enumeration">
          <tr>
            <td>
              <xsl:value-of select="@value" />
            </td>
          </tr>
        </xsl:for-each>
      </table>
    </div>
  </xsl:template>
  <xsl:template match="@* | node()">
    <xsl:apply-templates select="@* | node()" />
  </xsl:template>
</xsl:stylesheet>