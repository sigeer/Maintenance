# Maintenance

## 使用命令

### 生成更新包

```
mtnc pack
          -d, --dir 资源目录， 默认为当前工作目录
          -o, --output 生成的更新包
          -c, --config 生成更新包使用的配置文件
```

### 更新

```
mtnc update
          -d, --dir 需要更新的目录，默认为当前工作目录
          -u, --url 更新包的下载路径
          -p, --path 更新包的文件路径 两者选其一
```

### 卸载

删除当前工作目录下的所有文件
```
mtnc uninstall
```

## 逻辑步骤

### 更新

#### mtnc pack
将文件按照以下结构，打包压缩
```
├──_d
│   ├─aaa
│   ├─bbb
│   └─ccc
│——packages.mtncc
│——list
```
其中 packages.mtncc 文件是补丁包的信息
list文件 则是需要更新的文件名（list文件需要标注[替换*][新增+][移除-]）

#### mtnc update

1. 解压缩补丁包到临时目录
2. 根据list文件，备份会被替换，移除的本地文件
3. 将_d目录下的文件覆盖替换本地文件
4. 如果失败，则回滚


## 功能实现

- [ ] 更新备份，回滚
- [ ] 更新时，定制的处理，比如额外要执行的命令，脚本...(清理注册表，停止应用再更新...)