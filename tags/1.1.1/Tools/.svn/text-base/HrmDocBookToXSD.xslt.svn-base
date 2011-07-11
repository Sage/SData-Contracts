<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
    xmlns:xs="http://www.w3.org/2001/XMLSchema"
    xmlns:sme="http://schemas.sage.com/sdata/sme/2007"
                >
  <xsl:output method="xml" indent="yes" encoding="utf-8"/>

  <xsl:variable name="lo" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="up" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
  
  <xsl:template match="/article">
    <xsl:processing-instruction name="xml-stylesheet">
      <xsl:text>type="text/xsl" href="XsdToHtml.xslt"</xsl:text>
    </xsl:processing-instruction>
    <xs:schema id="hrmErp" 
               xmlns="http://schemas.sage.com/hrmErp/2009"
               elementFormDefault="qualified" 
               targetNamespace="http://schemas.sage.com/hrmErp/2009"
               xmlns:hrmErp="http://schemas.sage.com/hrmErp/2009"
               xmlns:sc="http://schemas.sage.com/sc/2009"
               xmlns:crmErp="http://schemas.sage.com/crmErp/2008"
               xmlns:famErp="http://schemas.sage.com/famErp/2009"
               xmlns:sme="http://schemas.sage.com/sdata/sme/2007"
               xmlns:xs="http://www.w3.org/2001/XMLSchema">
      <xs:import namespace="http://schemas.sage.com/sc/2009" />
      <xs:import namespace="http://schemas.sage.com/crmErp/2008" />
      <xs:import namespace="http://schemas.sage.com/famErp/2009" />
      <xsl:apply-templates select="sect1/sect2" mode="resourceKinds"/>
      <xsl:apply-templates select="sect1/sect2" mode="values"/>
      <xs:simpleType  name="uuid--type">
        <xs:restriction base="xs:string">
          <xs:pattern value="[0-9a-fA-F-]+"/>
        </xs:restriction>
      </xs:simpleType>
    </xs:schema>
  </xsl:template>

  <xsl:template match="sect1/sect2" mode="resourceKinds">
    <xsl:variable name="name" select="translate(title, '0123456789. &#160;&#10;','')" />
    <xsl:variable name="camel1" select="concat(translate(substring($name,1,1),$up,$lo),substring($name,2))"/>
    <xsl:variable name="pluralName" select="substring-before(substring-after($camel1,'pluralName='),',')"/>
    <xsl:variable name="compliance" select="substring-before(substring-after($camel1,'compliance='),',')"/>
    <xsl:variable name="canGet" select="contains($camel1,'canGet=TRUE')"/>
    <xsl:variable name="canPut" select="contains($camel1,'canPost=TRUE')"/>
    <xsl:variable name="canPost" select="contains($camel1,'canPut=TRUE')"/>
    <xsl:variable name="canDelete" select="contains($camel1,'canDelete=TRUE')"/>
    <xsl:variable name="camel" select="substring-before($camel1,',')" />
    <xsl:variable name="index" select="position()"/>
    <xsl:if test="$name != ''">
      <xs:element name="{$camel}" type="{$camel}--type" sme:role="resourceKind">
        <xsl:attribute name="sme:pluralName">
          <xsl:value-of select="$pluralName"/>
        </xsl:attribute>
        <xsl:if test="$compliance">
          <xsl:attribute name="sme:compliance">
            <xsl:value-of select="translate($compliance,$up,$lo)"/>
          </xsl:attribute>
        </xsl:if>
        <xsl:if test="$canGet">
          <xsl:attribute name="sme:canGet">true</xsl:attribute>
        </xsl:if>
        <xsl:if test="$canPut">
          <xsl:attribute name="sme:canPut">true</xsl:attribute>
        </xsl:if>
        <xsl:if test="$canPost">
          <xsl:attribute name="sme:canPost">true</xsl:attribute>
        </xsl:if>
        <xsl:if test="$canDelete">
          <xsl:attribute name="sme:canDelete">true</xsl:attribute>
        </xsl:if>
      </xs:element>
      <xs:complexType name="{$camel}--type">
        <xs:all>
          <xsl:apply-templates select="informaltable/tgroup/tbody/row[position() > 1]">
            <xsl:with-param name="resourceKind" select="$camel"/>
          </xsl:apply-templates>
        </xs:all>
      </xs:complexType>

      <xs:complexType name="{$camel}--list">
        <xs:sequence minOccurs="0" maxOccurs="unbounded">
          <xs:element name="{$camel}" type="{$camel}--type" />
        </xs:sequence>
      </xs:complexType>
    </xsl:if>

  </xsl:template>

  <xsl:template match="row">
    <xsl:param name="resourceKind"/>

      <xsl:variable name="label" select="translate(entry[1]/para, '&#10;', '')"/>
    <xsl:variable name="name" select="entry[2]/para"/>
    <xsl:variable name="description" select="string(entry[3]/para)"/>
    <xsl:variable name="isUnique" select="entry[4]/para = 'Yes'"/>
    <xsl:variable name="isExtension" select="entry[5]/para = 'Extended'"/>
    <xsl:variable name="nillable" select="entry[6]/para != 'No'"/>
    <xsl:variable name="isMandatory" select="entry[7]/para = 'True'"/>
    <xsl:variable name="type" select="translate(entry[8]/para, ' ', '')"/>
    <xsl:variable name="linkType" select="translate(string(entry[8]/para), '&#10; ', '')"/>
    <xsl:variable name="relationship" select="translate(entry[9]/para,' &#160;','')"/>
    <xsl:variable name="isCollection" select="entry[10]/para = 'true'"/>
    <xsl:variable name="comments" select="string(entry[11])"/>
    <xsl:variable name="values" select="entry[12]/para"/>

    <xsl:variable name="camelType" select="translate(concat(translate(substring($type,1,1),$up,$lo),substring($type,2)),' ','')"/>

    <xsl:if test="$name != ''">
      <xs:element name="{$name}" sme:label="{$label}" minOccurs="0">
        <xsl:choose>
          <xsl:when test="$type = 'String' and count(entry[12]/para) > 1">
            <xsl:variable name="capitalized" select="translate(concat(translate(substring($name,1,1),$lo,$up),substring($name,2)),' ','')"/>
            <xsl:choose>
              <xsl:when test="$resourceKind='salesOrderDelivery' and $capitalized='Status'">
                <xsl:attribute name="type"><xsl:value-of select="concat($resourceKind,$capitalized,'2--enum')"/></xsl:attribute>
              </xsl:when>
              <xsl:otherwise>
                <xsl:attribute name="type"><xsl:value-of select="concat($resourceKind,$capitalized,'--enum')"/></xsl:attribute>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:when>
          <xsl:when test="$type = 'String'">
            <xsl:attribute name="type">xs:string</xsl:attribute>
          </xsl:when>
          <xsl:when test="$type = 'uuid'">
            <xsl:attribute name="type">uuid--type</xsl:attribute>
          </xsl:when>
          <xsl:when test="$type = 'Boolean'">
            <xsl:attribute name="type">xs:boolean</xsl:attribute>
          </xsl:when>
          <xsl:when test="$type = 'Decimal'">
            <xsl:attribute name="type">xs:decimal</xsl:attribute>
          </xsl:when>
          <xsl:when test="$type = 'Datetime'">
            <xsl:attribute name="type">xs:date</xsl:attribute>
          </xsl:when>
          <xsl:when test="$type = 'Time'">
            <xsl:attribute name="type">xs:time</xsl:attribute>
          </xsl:when>
          <xsl:when test="contains($type,'/')">

            <xsl:variable name="foreignType" select="substring-after($type,'/')"/>
            <xsl:variable name="foreignTypeCamel" select="concat(translate(substring($foreignType,1,1),$up,$lo),substring($foreignType,2))"/>
            <xsl:variable name="foreignTypePrefixed" select="concat(substring-before($type,'/'),':',$foreignTypeCamel)"/>
            <xsl:choose>
              <xsl:when test="$isCollection">
                <xsl:attribute name="type"><xsl:value-of select="$foreignTypePrefixed"/>--list</xsl:attribute>
              </xsl:when>
              <xsl:otherwise>
                <xsl:attribute name="type"><xsl:value-of select="$foreignTypePrefixed"/>--type</xsl:attribute>
              </xsl:otherwise>
            </xsl:choose>

          </xsl:when>
          <xsl:otherwise>
            <xsl:variable name="camelLinkType" select="translate(concat(translate(substring($linkType,1,1),$up,$lo),substring($linkType,2)),' ','')"/>
            <xsl:choose>
              <xsl:when test="$isCollection">
                <xsl:attribute name="type"><xsl:value-of select="$camelLinkType"/>--list</xsl:attribute>
              </xsl:when>
              <xsl:otherwise>
                <xsl:attribute name="type"><xsl:value-of select="$camelLinkType"/>--type</xsl:attribute>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:otherwise>
        </xsl:choose>

        <xsl:if test="$relationship != ''">
          <xsl:attribute name="sme:relationship">
            <xsl:value-of select="$relationship"/>
          </xsl:attribute>
          <xsl:attribute name="sme:isCollection">
            <xsl:value-of select="$isCollection"/>
          </xsl:attribute>
        </xsl:if>

        <xsl:if test="$isUnique">
          <xsl:attribute name="sme:isUniqueKey">true</xsl:attribute>
        </xsl:if>

        <xsl:if test="$isExtension">
          <xsl:attribute name="sme:isExtension">true</xsl:attribute>
        </xsl:if>

        <xsl:if test="$isMandatory">
          <xsl:attribute name="sme:isMandatory">true</xsl:attribute>
        </xsl:if>

        <xsl:if test="$nillable">
          <xsl:attribute name="nillable">true</xsl:attribute>
        </xsl:if>
        <xs:annotation>
          <xs:documentation>
            <xsl:value-of select="$description"/>
            <xsl:if test="$comments != ''">
              &#160;<xsl:value-of select="$comments"/>
            </xsl:if>
          </xs:documentation>
        </xs:annotation>
      </xs:element>
    </xsl:if>
  </xsl:template>

  <xsl:template match="sect1/sect2" mode="values">
    <xsl:variable name="kindName" select="translate(substring-before(title,','), '0123456789. &#160;&#10;','')"/>
    <xsl:for-each select="informaltable/tgroup/tbody/row[position() > 1]/entry[12]">
      <xsl:if test="count(para) > 1">
        <xsl:variable name="kind" select="ancestor::sect2"/>
        <xsl:variable name="name" select="parent::row/entry[2]/para"/>
        <xsl:variable name="capitalized" select="translate(concat(translate(substring($name,1,1),$lo,$up),substring($name,2)),' ','')"/>
        <xsl:variable name="camel" select="translate(concat(translate(substring($kindName,1,1),$up,$lo),substring($kindName,2)),' ','')"/>
        <xs:simpleType>
          <xsl:choose>
            <xsl:when test="$camel='salesOrderDelivery' and $capitalized='Status'">
              <xsl:attribute name="name">
                <xsl:value-of select="concat($camel,$capitalized,'2--enum')"/>
              </xsl:attribute>
            </xsl:when>
            <xsl:otherwise>
              <xsl:attribute name="name">
                <xsl:value-of select="concat($camel,$capitalized,'--enum')"/>
              </xsl:attribute>
            </xsl:otherwise>
          </xsl:choose>
          <xs:restriction base="xs:string">
            <xsl:for-each select="para[text() != '']">
              <xs:enumeration>
                <xsl:attribute name="value"><xsl:value-of select="node()"/></xsl:attribute>
              </xs:enumeration>
            </xsl:for-each>
          </xs:restriction>
        </xs:simpleType>
      </xsl:if>
    </xsl:for-each>
  </xsl:template>

    <xsl:template match="@* | node()">
    <xsl:apply-templates select="@* | node()"/>
  </xsl:template>
</xsl:stylesheet>
