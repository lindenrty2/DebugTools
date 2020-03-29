using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Diagnostics;
using Microsoft.VisualBasic;
using System.Linq;
using System.Windows.Documents;
using System.Collections.Generic;
using System.Collections;
using System.Windows.Media;
using System.Windows.Controls;
using System;
using System.Xml.Linq;
using System.Windows.Navigation;
using System.IO;
using System.Runtime.InteropServices;
using DebugTools.Package;
using DebugTools.DataBase;
using DebugTools.Secret;

namespace DebugTools.DBO
{
    public class PackageFile
    {
        private PackageMate _mate;
        public PackageMate Mate
        {
            get
            {
                return _mate;
            }
        }

        private IEnumerable<IBlockInfo> _blockInfoList = null;
        public IEnumerable<IBlockInfo> BlockInfos
        {
            get
            {
                if (!IsLoadFormFile)
                    throw new InvalidDataException("FileからLoadされた場合のみ使える");
                if (_blockInfoList == null)
                    _blockInfoList = ReadBlockInfos();
                return _blockInfoList;
            }
        }

        private PackageHeader _header;
        public PackageHeader Header
        {
            get
            {
                if (_header == null)
                    _header = (PackageHeader)BlockInfos.First();
                return _header;
            }
        }

        private List<PackageTableInfo> _tables;
        public IEnumerable<PackageTableInfo> Tables
        {
            get
            {
                if (_tables == null)
                    _tables = ReadElementInfos<PackageTableInfo>(PackageBlockType.Table).ToList();
                return _tables;
            }
        }

        private List<PackageViewInfo> _views;
        public IEnumerable<PackageViewInfo> Views
        {
            get
            {
                return _views;
            }
        }

        public string DisplayName
        {
            get
            {
                return string.Format("チップ：{0}  認証キー：{1}  パッケージ", this.Mate.Tip, this.Mate.KeyName, this.Mate.Version);
            }
        }

        private string _path;
        public string Path
        {
            get
            {
                return _path;
            }
        }

        private bool _isLoadFormFile = false;
        public bool IsLoadFormFile
        {
            get
            {
                return _isLoadFormFile;
            }
        }

        private int _blockIndex = 0;
        private IDataAccessor _dataAccessor;
        private FileStream _stream;
        private ISecretService _secretService;
        public ISecretService SecretService
        {
            get
            {
                return _secretService;
            }
        }

        public IDataAccessor DataAccessor
        {
            get
            {
                return _dataAccessor;
            }
        }

        public PackageFile(IDataAccessor dataAccessor)
        {
            _dataAccessor = dataAccessor;
            PackageMate newMate = new PackageMate();
            newMate.Version = Application.GetPackageVersionNo();
            _mate = newMate;
            _header = new PackageHeader(this);
            _tables = new List<PackageTableInfo>();
        }

        public PackageFile(IDataAccessor dataAccessor, string path)
        {
            try
            {
                _dataAccessor = dataAccessor;
                _stream = File.OpenRead(path);
                this._isLoadFormFile = true;
                PackageMate? mate = this.ReadMate();
                if (!mate.HasValue)
                    throw new InvalidDataException("データ構造不正、解読できない");
                this._mate = mate.Value;
                this._secretService = SecretServiceCenter.GetInstance().GetSecretService((SecretMode)this._mate.SecretMode, this._mate.KeyName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Close();
                throw ex;
            }
            finally
            {
            }
        }

        public void Close()
        {
            try
            {
                if (_stream != null)
                    _stream.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public int GetNextBlockIndex()
        {
            _blockIndex += 1;
            return _blockIndex;
        }

        public void Add(PackageTableInfo table)
        {
            _tables.Add(table);
        }

        public void Add(PackageViewInfo view)
        {
            _views.Add(view);
        }

        public PackageMate? ReadMate()
        {
            try
            {
                _stream.Position = 0;
                int size = Marshal.SizeOf(typeof(PackageMate));
                byte[] bytes = new byte[size - 1 + 1];
                _stream.Read(bytes, 0, size);
                return ConvertHelper.BytesToStruct<PackageMate>(bytes);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return default(PackageMate?);
            }
        }

        public IEnumerable<IBlockInfo> ReadBlockInfos()
        {
            try
            {
                _stream.Position = Marshal.SizeOf(typeof(PackageMate));
                int count = this.Mate.BlockCount;
                int size = Marshal.SizeOf(typeof(PackageBlockInfo));
                List<IBlockInfo> blockInfoList = new List<IBlockInfo>();
                var loopTo = count - 1;
                for (int index = 0; index <= loopTo; index++)
                {
                    byte[] bytes = new byte[size - 1 + 1];
                    _stream.Read(bytes, 0, size);
                    PackageBlockInfo blockInfo = ConvertHelper.BytesToStruct<PackageBlockInfo>(bytes);
                    blockInfoList.Add(blockInfo);
                }
                return blockInfoList;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public IEnumerable<T> ReadElementInfos<T>(PackageBlockType type) where T : PackageSerializable
        {
            try
            {
                List<T> list = new List<T>();
                foreach (IBlockInfo i in this.BlockInfos)
                {
                    if (!(i.Type == (short)type))
                        continue;
                    T element = ReadElement<T>(i);
                    if (element == null)
                        continue;
                    list.Add(element);
                }
                return list;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public T ReadElement<T>(int index) where T : PackageSerializable
        {
            if (index < 0)
                return null;
            IBlockInfo blockInfo = _blockInfoList.FirstOrDefault(x => x.Index == index);
            if (blockInfo == null)
                return null;
            return ReadElement<T>(blockInfo);
        }

        public T ReadElement<T>(IBlockInfo blockInfo) where T : PackageSerializable
        {
            try
            {
                _stream.Position = blockInfo.StartPosition;
                byte[] bytes = new byte[blockInfo.Size - 1 + 1];
                _stream.Read(bytes, 0, blockInfo.Size);
                if (SecretService != null)
                    bytes = SecretService.Dencrypt(bytes);
                System.Reflection.ConstructorInfo ci = typeof(T).GetConstructor(new[] { typeof(PackageFile), typeof(IBlockInfo), typeof(byte[]) });
                return (T)ci.Invoke(new object[] { this, blockInfo, bytes });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public bool Write(FileStream stream, ISecretService secretService)
        {
            List<PackageSerializable> blockList = new List<PackageSerializable>();
            blockList.Add(Header);
            foreach (PackageTableInfo element in this._tables)
            {
                blockList.Add(element);
                blockList.Add(element.Data);
            }
            this._mate.BlockCount = blockList.Count;
            this._mate.SecretMode = secretService == null ? (short)0 : (short)secretService.SecretMode;
            this._mate.KeyName = secretService == null ? string.Empty : secretService.KeyName;
            // 位置を確保するため、まず仮の内容で一回Writeする
            if (!WriteMate(stream))
                return false;
            if (!WriteBlockInfos(stream, blockList))
                return false;
            // 内容部分
            if (!WriteBlocks(stream, blockList, secretService))
                return false;
            // 真の情報をWriteする
            stream.Position = 0;
            this._mate.BlockStartPosition = Header.BlockInfo.StartPosition;
            if (!WriteMate(stream))
                return false;
            if (!WriteBlockInfos(stream, blockList))
                return false;
            return true;
        }

        private bool WriteMate(FileStream stream)
        {
            try
            {
                byte[] bytes = ConvertHelper.StructToBytes(this.Mate);
                stream.Write(bytes, 0, bytes.Length);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        private bool WriteBlockInfos(FileStream stream, IEnumerable<PackageSerializable> elements)
        {
            try
            {
                foreach (PackageSerializable element in elements)
                    WriteBlockList(stream, element);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        private bool WriteBlockList(FileStream stream, PackageSerializable element)
        {
            try
            {
                byte[] bytes = ConvertHelper.StructToBytes(element.BlockInfo);
                stream.Write(bytes, 0, bytes.Length);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        private bool WriteBlocks(FileStream stream, IEnumerable<PackageSerializable> elements, ISecretService secretService)
        {
            try
            {
                foreach (PackageSerializable element in elements)
                    WriteBlock(stream, element, secretService);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        private bool WriteBlock(FileStream stream, PackageSerializable element, ISecretService secretService)
        {
            try
            {
                byte[] bytes = element.ToByte();
                if (secretService != null)
                    bytes = secretService.Encrypt(bytes);
                element.BlockInfo = new PackageBlockInfo(element.BlockIndex, stream.Position, bytes.Length, element.Type);
                stream.Write(bytes, 0, bytes.Length);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
    }
}
