﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AmadeusRSAKeys.RsaKeysService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="urn:rsa_key", ConfigurationName="RsaKeysService.rsa_keyPortType")]
    public interface rsa_keyPortType {
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:rsa_key#login", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(Style=System.ServiceModel.OperationFormatStyle.Rpc, SupportFaults=true, Use=System.ServiceModel.OperationFormatUse.Encoded)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="return")]
        string login(string terminal, [System.ServiceModel.MessageParameterAttribute(Name="login")] string login1, string password);
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:rsa_key#logout", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(Style=System.ServiceModel.OperationFormatStyle.Rpc, SupportFaults=true, Use=System.ServiceModel.OperationFormatUse.Encoded)]
        void logout(string session_id);
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:rsa_key#login_token", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(Style=System.ServiceModel.OperationFormatStyle.Rpc, SupportFaults=true, Use=System.ServiceModel.OperationFormatUse.Encoded)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="return")]
        string login_token(string terminal, string token);
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:rsa_key#get_new_token", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(Style=System.ServiceModel.OperationFormatStyle.Rpc, SupportFaults=true, Use=System.ServiceModel.OperationFormatUse.Encoded)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="return")]
        string get_new_token(string terminal, string old_token);
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:rsa_key#changeKeyToken", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(Style=System.ServiceModel.OperationFormatStyle.Rpc, SupportFaults=true, Use=System.ServiceModel.OperationFormatUse.Encoded)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="message")]
        string changeKeyToken(string SessionId, string OldKey, string NewKey);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface rsa_keyPortTypeChannel : AmadeusRSAKeys.RsaKeysService.rsa_keyPortType, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class rsa_keyPortTypeClient : System.ServiceModel.ClientBase<AmadeusRSAKeys.RsaKeysService.rsa_keyPortType>, AmadeusRSAKeys.RsaKeysService.rsa_keyPortType {
        
        public rsa_keyPortTypeClient() {
        }
        
        public rsa_keyPortTypeClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public rsa_keyPortTypeClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public rsa_keyPortTypeClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public rsa_keyPortTypeClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string login(string terminal, string login1, string password) {
            return base.Channel.login(terminal, login1, password);
        }
        
        public void logout(string session_id) {
            base.Channel.logout(session_id);
        }
        
        public string login_token(string terminal, string token) {
            return base.Channel.login_token(terminal, token);
        }
        
        public string get_new_token(string terminal, string old_token) {
            return base.Channel.get_new_token(terminal, old_token);
        }
        
        public string changeKeyToken(string SessionId, string OldKey, string NewKey) {
            return base.Channel.changeKeyToken(SessionId, OldKey, NewKey);
        }
    }
}
