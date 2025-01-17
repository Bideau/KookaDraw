﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Collections;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using KukaDraw.Core;

namespace KukaDraw.Brain
{
    class SVGParser
    {
        private string[] dataList;
        private string data;
        string isEmpty = Constants.stringIsEmpty;

        public SVGParser()
        {
            this.dataList = null;
            this.data = null;
        }

        public string[] GetDataList()
        {
            return dataList;
        }

        public void SetDataList(string[] dataList)
        {
            this.dataList = dataList;
        }
        public void SvgXmlReader(Stream xmlStream)
        {
            var reader = new XmlTextReader(xmlStream);
            reader.ProhibitDtd = false;
            reader.DtdProcessing = DtdProcessing.Ignore;
            reader.XmlResolver = null;

            var doc = new XmlDocument();
            doc.XmlResolver = null;
            doc.Load(reader);

            foreach (XmlElement node in doc.GetElementsByTagName(Constants.path))
            {
                //Console.WriteLine(node.GetAttribute("d") + " titi " + "\n");
                this.data = this.data + Constants.stringSpace + "Y" + Constants.stringSpace + node.GetAttribute(Constants.pathValue);
            }
            //suppression de tout les z dans les data
            this.data = this.data.Replace("z", isEmpty);
            this.data = this.data.Replace("Z", isEmpty);
            this.data = this.data.Replace("\n", Constants.stringSpace);
            this.data = this.data.Replace(",", Constants.stringSpace);
            
            //Console.WriteLine(this.data);

        }
        public string addSeparator(string chain)
        {
            string tmpChain = null;

            tmpChain = chain[0] + Constants.stringSpace;

            for (int i = 1; i < chain.Length; i++)
            {
                if (chain[i].Equals('m') || chain[i].Equals('M') || chain[i].Equals('c') || chain[i].Equals('C') || chain[i].Equals('l') || chain[i].Equals('L'))
                {
                    tmpChain = tmpChain + chain[i] + Constants.stringSpace;
                }
                else
                {
                    tmpChain = tmpChain + chain[i];
                }
            }
            return tmpChain;
        }

        public void Parse()
        {
            string[] separator = { Constants.stringSpace };
            string[] separators = { "m", "M" };
            string[] spliters = this.data.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            string moveTo = null;
            string tmp = null;

            for (int i = 1; i < spliters.Length; i++)
            {
                moveTo = "m" + spliters[i];           
                tmp = tmp + addSeparator(moveTo);
            }
           this.dataList = tmp.Split(separator, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
