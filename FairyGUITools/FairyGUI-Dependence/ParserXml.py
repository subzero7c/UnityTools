#!/usr/bin/env pytho
# -*- coding: utf-8 -*-
# 解析FairyGUI依赖关系

# 1、遍历所有Package.Xml  记录所有资源ID 以及CompoentID
# 2、遍历所有package中的xml
# 3、统计Package中来自其他Package的资源
# 4、统计图片来自非本包的引用次数和来源


#
#    Package   Component  OtherPackage  otherComponent or otherImage
#
#


import xml.etree.ElementTree as ET


class ParserPackage():
    def packagePath(self, resPath):
        tree = ET.parse(resPath)
        # 根节点
        root = tree.getroot()
        # 标签名
        print(root.tag)
        print(root[0])
        # print(root[1].text)
        for str in root[0].findall("image"):
            # print(str.t)
            print(str.attrib["id"])
            # print(str.attrib["component"])
        print("component =================")
        for str in root[0].findall("component"):
            print(str.attrib["id"])


# targetPath="F:\WorkSpace\Unity\Company\FariyGUI\assets"
parser = ParserPackage()
parser.packagePath("F:/WorkSpace/Unity/Company/FariyGUI/assets/Build/package.xml")
